using System;
using System.Collections.Generic;
using System.Linq;
using BaseApp.Core.Helpers;
using BaseApp.Core.Services.CommonServices;
using BaseApp.Data.DbModels;
using BaseApp.Data.DbModels.JoinedModels;
using BaseApp.Data.Requests;
using BaseApp.Data.Responses;
using BaseApp.InjectionServices;
using BaseApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Controllers
{
    [ApiController]
    [Route("article")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ArticleController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly IRepositoryBehavior _articleRepository;
        private readonly IRepositoryBehavior _userRepository;

        public ArticleController(IRepositoryFactory repository,
            IAuditService auditService)
        {
            _auditService = auditService;
            _articleRepository = repository.Choose(typeof(ArticleRepository));
            _userRepository = repository.Choose(typeof(AuthenticationUserRepository));
        }

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var dbArticles = _articleRepository.GetAll() as IEnumerable<ArticleAuditDbModel> 
                             ?? new List<ArticleAuditDbModel>();
            return StatusCode(200, dbArticles.Select(r => new ArticleResponse(r)));
        }
        
        [HttpGet]
        public IActionResult GetArticle([FromQuery] string id)
        {
            var dbModel = _articleRepository.GetById(id) as ArticleAuditDbModel;
            if (dbModel == null)
            {
                return StatusCode(404, "El articulo que intentas visualizar ya no existe");
            }
            return StatusCode(200, new ArticleResponse(dbModel));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Developer")]
        public IActionResult CreateArticle([FromForm] ArticleRequest article)
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbUser = _userRepository.GetById(userId) as AuthenticationUserAuditDbModel;
            if (dbUser == null)
            {
                return StatusCode(401, "El token expiró");
            }
            JwtService.ValidateJwtVersion(dbUser.AuthenticationDb, User);
            PriceService.ValidatePrices(article);
            var ipAddress = IpService.GetIpAddress(Request);
            var auditFields = _auditService.GetNewAuditFields(dbUser.UserDb.Id, ipAddress);
            var articleModel = new ArticleDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Description = article.Description,
                Discount = article.Discount,
                Name = article.Name,
                Price = article.Price,
                Total = article.Price - article.Discount,
                AuditDbModelId = auditFields.Id,
            };
            articleModel.PointValue = articleModel.Total * 0.01M;
            var auditModel = new ArticleAuditDbModel
            {
                ArticleDb = articleModel,
                AuditDb = auditFields
            };
            _articleRepository.Create(auditModel);
            return StatusCode(200, new ArticleResponse(auditModel));
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Developer")]
        public IActionResult UpdateArticle([FromForm] ArticleRequest article)
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbUser = _userRepository.GetById(userId) as AuthenticationUserAuditDbModel;
            if (dbUser == null)
            {
                return StatusCode(401, "El token expiró");
            }
            JwtService.ValidateJwtVersion(dbUser.AuthenticationDb, User);
            PriceService.ValidatePrices(article);
            var dbArticle = _articleRepository.GetById(article.Id) as ArticleAuditDbModel;
            if (dbArticle == null)
            {
                return StatusCode(404, "El articulo que intentas editar ya no existe");
            }
            var ip = IpService.GetIpAddress(Request);
            var auditFields = _auditService.UpdateAuditFields(dbArticle.AuditDb, dbUser.AuthenticationDb.Email, ip);
            dbArticle.AuditDb = auditFields;
            var updatedArticle = article.UpdateDbModelValues(dbArticle);
            _articleRepository.Update(updatedArticle);
            return StatusCode(200, new ArticleResponse(updatedArticle));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Developer")]
        public IActionResult DeleteArticle([FromQuery] string id)
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbUser = _userRepository.GetById(userId) as AuthenticationUserAuditDbModel;
            if (dbUser == null)
            {
                return StatusCode(401, "El token expiró");
            }
            JwtService.ValidateJwtVersion(dbUser.AuthenticationDb, User);
            var dbArticle = _articleRepository.GetById(id) as ArticleAuditDbModel;
            if (dbArticle == null)
            {
                return StatusCode(404, "El articulo que intentas eliminar ya no existe");
            }
            _articleRepository.Delete(dbArticle);
            return StatusCode(200, new MessageResponse("El articulo se elimino con exitó"));
        }
    }
}