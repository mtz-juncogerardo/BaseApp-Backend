using System.Collections.Generic;
using System.Linq;
using BaseApp.Core.Helpers;
using BaseApp.Core.Services.CommonServices;
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
    [Route("user")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    public class UserController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly IRepositoryBehavior _authenticationUserRepository;

        public UserController(IAuditService auditService, IRepositoryFactory repository)
        {
            _auditService = auditService;
            _authenticationUserRepository = repository.Choose(typeof(AuthenticationUserRepository));
        }
        
        [HttpGet("all")]
        [Authorize(Roles = "Admin, Developer")]
        public IActionResult GetAllUsers()
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _authenticationUserRepository.GetById(userId) as AuthenticationUserAuditDbModel;
            if (dbModel == null)
            {
                return StatusCode(401, "El token expiró");
            }
            JwtService.ValidateJwtVersion(dbModel.AuthenticationDb, User);
            var dbUsers = _authenticationUserRepository.GetAll() as IEnumerable<AuthenticationUserAuditDbModel>;
            return StatusCode(200, dbUsers?.Select(r => new UserResponse(r)));
        }

        [HttpGet]
        public IActionResult GetMyUser()
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _authenticationUserRepository.GetById(userId) as AuthenticationUserAuditDbModel;
            if (dbModel == null)
            {
                return StatusCode(401, "El token expiró");
            }
            return StatusCode(200, new UserResponse(dbModel));
        }

        [HttpPut]
        public IActionResult UpdateUser([FromForm] UserRequest request)
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _authenticationUserRepository.GetById(userId) as AuthenticationUserAuditDbModel;
            if (dbModel == null)
            {
                return StatusCode(401, "El token expiró");
            }
            var ip = IpService.GetIpAddress(Request);
            var audit = _auditService.UpdateAuditFields(dbModel.AuditDb, dbModel.AuthenticationDb.Email, ip);
            dbModel.AuditDb = audit;
            var updatedDbModel = request.UpdateDbModelValues(dbModel);
            _authenticationUserRepository.Update(updatedDbModel);
            return StatusCode(200, new UserResponse(updatedDbModel));
        }

        [HttpDelete]
        public IActionResult DeleteUser()
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _authenticationUserRepository.GetById(userId);
            if (dbModel == null)
            {
                return StatusCode(401, "El token expiró");
            }
            _authenticationUserRepository.Delete(dbModel);
            return StatusCode(200, new MessageResponse("El usuario se elimino de nuestra base de datos"));
        }
    }
}