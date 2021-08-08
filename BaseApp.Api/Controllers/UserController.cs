using System.Collections.Generic;
using System.Linq;
using BaseApp.Core.Helpers;
using BaseApp.Core.Services.CommonServices;
using BaseApp.Data.Models;
using BaseApp.Data.Repositories;
using BaseApp.Data.Requests;
using BaseApp.Data.Responses;
using BaseApp.InjectionServices;
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
        private readonly IRepositoryBehavior _userRepository;

        public UserController(IAuditService auditService, IRepositoryFactory repository)
        {
            _auditService = auditService;
            _userRepository = repository.Choose(typeof(UserRepository));
        }
        
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _userRepository.GetById(userId) as AuthenticationUserAuditModel ?? new AuthenticationUserAuditModel();
            JwtService.ValidateJwtVersion(dbModel.AuthenticationDb, User);
            var dbUsers = _userRepository.GetAll() as IEnumerable<AuthenticationUserAuditModel>;
            return StatusCode(200, dbUsers?.Select(r => new UserResponse(r)));
        }

        [HttpGet]
        public IActionResult GetMyUser()
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _userRepository.GetById(userId) as AuthenticationUserAuditModel;
            return StatusCode(200, new UserResponse(dbModel));
        }

        [HttpPut]
        public IActionResult UpdateUser([FromForm] UserRequest request)
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _userRepository.GetById(userId) as AuthenticationUserAuditModel;
            if (dbModel == null)
            {
                return StatusCode(401, "El token expiró");
            }
            var ip = IpService.GetIpAddress(Request);
            var audit = _auditService.UpdateAuditFields(dbModel.AuditDb, dbModel.AuthenticationDb.Email, ip);
            var authUserAudit = new AuthenticationUserAuditModel
            {
                AuditDb = audit,
                AuthenticationDb = dbModel.AuthenticationDb,
                UserDb = dbModel.UserDb
            };
            var updatedDbModel = request.ConverToDbModel(authUserAudit);
            _userRepository.Update(updatedDbModel);
            return StatusCode(200, new UserResponse(authUserAudit));
        }

        [HttpDelete]
        public IActionResult DeleteUser()
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _userRepository.GetById(userId);
            if (dbModel == null)
            {
                return StatusCode(401, "El token expiró");
            }
            _userRepository.Delete(dbModel);
            return StatusCode(200, new MessageResponse("El usuario se elimino de nuestra base de datos"));
        }
    }
}