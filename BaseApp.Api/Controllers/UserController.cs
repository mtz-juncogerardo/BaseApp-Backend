using System.Collections.Generic;
using System.Linq;
using BaseApp.Core.Helpers;
using BaseApp.Core.Services.CommonServices;
using BaseApp.Data.DbModels;
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
        private readonly IConfigurationService _configuration;
        private readonly IAuditService _auditService;
        private readonly IUserRepository _userRepository;

        public UserController(IConfigurationService configuration,
            IAuditService auditService,
            IUserRepository userRepository)
        {
            _configuration = configuration;
            _auditService = auditService;
            _userRepository = userRepository;
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _userRepository.GetById(userId);
            JwtService.ValidateJwtVersion(dbModel.AuthenticationDb, User);

            var dbUsers = _userRepository.All
                .Select(r => new UserResponse(r));
            return StatusCode(200, dbUsers);
        }

        [HttpGet]
        public IActionResult GetMyUser()
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _userRepository.GetById(userId);
            return StatusCode(200, new UserResponse(dbModel));
        }

        [HttpPut]
        public IActionResult UpdateUser([FromForm] UserRequest request)
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _userRepository.GetById(userId);
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