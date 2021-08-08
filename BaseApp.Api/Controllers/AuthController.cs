using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BaseApp.Core.Helpers;
using BaseApp.Core.Services.CommonServices;
using BaseApp.Core.Services.MailService;
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
    [Route("auth")]
    [TypeFilter(typeof(ExceptionFilter))]
    public class AuthController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly IConfigurationService _configuration;
        private readonly IRepositoryBehavior _authenticationUserRepository;

        public AuthController(IRepositoryFactory repository,
            IConfigurationService configuration,
            IAuditService auditService)
        {
            _configuration = configuration;
            _auditService = auditService;
            _authenticationUserRepository = repository.Choose(typeof(AuthenticationUserRepository));
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpUser([FromForm] AuthenticationRequest request)
        {
            EmailValidator.ValidateEmail(request.Email);
            var dbAuthUserAudit = _authenticationUserRepository.GetByKeyValue("Email", request.Email)
                .FirstOrDefault() as AuthenticationUserAuditModel;
            if (dbAuthUserAudit != null && dbAuthUserAudit.AuditDb.CreatedAtDate < DateTime.Now.AddDays(1))
            {
                _authenticationUserRepository.Delete(dbAuthUserAudit);
                dbAuthUserAudit = null;
            }
            if (dbAuthUserAudit != null)
            {
                return StatusCode(405, "El email ya se encuentra registrado, intenta iniciar sesión");
            }
            var salt = PasswordService.GenerateSalt();
            var ipAddress = IpService.GetIpAddress(Request);
            var auditFields = _auditService.GetNewAuditFields(request.Email, ipAddress);
            var auth = new AuthenticationDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                Salt = salt,
                VersionId = Guid.NewGuid().ToString(),
                Password = PasswordService.GetHashPassword(request.Password, salt)
            };
            var user = new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                AuthenticationDbModel = auth,
                AuditDbModel = auditFields
            };
            _authenticationUserRepository.Create(user);
            var token = JwtService.GenerateTokenWithUserId(_configuration.JwtSecret,
                DateTime.Now.AddDays(1),
                user.Id,
                auth.VersionId);
            var mailArgs = new MailArgs
            {
                Subject = "Gracias por Registrarte!",
                Link = $"{_configuration.ClientUrl}validate?token={token}",
                ReceiverEmail = request.Email,
                SendGridApiKey = _configuration.SendGridApiKey
            };
            await MailFactory.SendMailTemplate(MailTemplateType.ConfirmEmail, mailArgs);
            return StatusCode(200, new MessageResponse("Para continuar da click en el enlace que te enviamos por correo."));
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromForm] AuthenticationRequest request)
        {
            EmailValidator.ValidateEmail(request.Email);
            var dbAuthUserAudit = _authenticationUserRepository.GetByKeyValue("Email", request.Email)
                .FirstOrDefault() as AuthenticationUserAuditModel;
            if (dbAuthUserAudit == null)
            {
                return StatusCode(401, "El usuario o la contraseña son incorrectos");
            }
            PasswordService.ValidatePasswordMatching(request.Password,
                dbAuthUserAudit.AuthenticationDb.Salt!,
                dbAuthUserAudit.AuthenticationDb.Password!);
            if (!dbAuthUserAudit.AuthenticationDb.EmailValid)
            {
                return StatusCode(401, "Debes validar tu correo antes de poder utilizar tu cuenta");
            }
            var token = JwtService.GenerateTokenWithUserId(_configuration.JwtSecret,
                DateTime.Now.AddDays(14),
                dbAuthUserAudit.UserDb.Id!,
                dbAuthUserAudit.AuthenticationDb.VersionId!,
                dbAuthUserAudit.AuthenticationDb.Role);
            return StatusCode(200, new AuthenticationResponse(token));
        }

        [HttpPost("recovery")]
        public async Task<IActionResult> RecoverUserCredentials([FromForm] AuthenticationRequest request)
        {
            EmailValidator.ValidateEmail(request.Email);
            var dbModel = _authenticationUserRepository.GetByKeyValue("Email", request.Email)
                .FirstOrDefault() as AuthenticationUserAuditModel;
            if (dbModel == null)
            {
                return StatusCode(404, "No se encontro ninguna cuenta asociada a ese correo");
            }
            dbModel.AuthenticationDb.VersionId = Guid.NewGuid().ToString();
            _authenticationUserRepository.Update(dbModel);
            var token = JwtService.GenerateTokenWithUserId(_configuration.JwtSecret,
                DateTime.Now.AddMinutes(5),
                dbModel.UserDb.Id!,
                dbModel.AuthenticationDb.VersionId);
            var mailArgs = new MailArgs
            {
                Subject = "Solicitaste un cambio de contraseña",
                Link = $"{_configuration.ClientUrl}recovery?token={token}",
                ReceiverEmail = request.Email,
                SendGridApiKey = _configuration.SendGridApiKey
            };
            await MailFactory.SendMailTemplate(MailTemplateType.PasswordRecovery, mailArgs);
            return StatusCode(200,
                new MessageResponse("Se envio un correo con las instrucciones para restablecer tu contraseña."));
        }

        [Authorize]
        [HttpPut("password")]
        public IActionResult ChangePassword([FromForm] AuthenticationRequest request)
        {
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _authenticationUserRepository.GetById(userId) as AuthenticationUserAuditModel ?? new AuthenticationUserAuditModel();
            JwtService.ValidateJwtVersion(dbModel.AuthenticationDb, User);
            var ip = IpService.GetIpAddress(Request);
            dbModel.AuthenticationDb.Salt = PasswordService.GenerateSalt();
            dbModel.AuthenticationDb.Password = PasswordService.GetHashPassword(request.Password,
                dbModel.AuthenticationDb.Salt);
            dbModel.AuditDb = _auditService.UpdateAuditFields(dbModel.AuditDb, dbModel.AuthenticationDb.Email, ip);
            _authenticationUserRepository.Update(dbModel);
            return StatusCode(200, new MessageResponse("Cambiaste tu contraseña con exitó"));
        }

        [Authorize]
        [HttpPost("email")]
        public async Task<IActionResult> RequestMailChange([FromForm] AuthenticationRequest request)
        {
            EmailValidator.ValidateEmail(request.Email);
            var userId = JwtService.GetClaimUserId(User);
            var dbModelAuth = _authenticationUserRepository.GetById(userId) as AuthenticationUserAuditModel;
            if (dbModelAuth?.AuthenticationDb.Email == request.Email)
            {
                return StatusCode(400, "El correo es identico al anterior");
            }
            var mailExists = _authenticationUserRepository.GetByKeyValue("Email", request.Email).Any();
            if (mailExists)
            {
                return StatusCode(400, "Este correo ya se encuentra registrado");
            }
            var claims = new List<Claim>
            {
                new("Email", request.Email),
                new("UserId", userId)
            };
            var token = JwtService.GenerateTokenWithClaims(_configuration.JwtSecret, DateTime.Now.AddMinutes(5), claims);
            var mailArgs = new MailArgs
            {
                Subject = "Confirma tu nuevo correo",
                Link = $"{_configuration.ClientUrl}email?token={token}",
                ReceiverEmail = request.Email,
                SendGridApiKey = _configuration.SendGridApiKey
            };
            await MailFactory.SendMailTemplate(MailTemplateType.EmailChange, mailArgs);
            return StatusCode(200,
                new MessageResponse("Se envio un enlace al nuevo correo, ingresa a el para completar los cambios"));
        }

        [Authorize]
        [HttpPut("email")]
        public IActionResult ChangeEmail()
        {
            var email = JwtService.GetClaims(User).FirstOrDefault(r => r.Type == "Email")?.Value;
            EmailValidator.ValidateEmail(email ?? string.Empty);
            var userId = JwtService.GetClaimUserId(User);
            var dbModel = _authenticationUserRepository.GetById(userId) as AuthenticationUserAuditModel;
            if (dbModel == null)
            {
                return StatusCode(404, "No se encontro el usuario");
            }
            var ip = IpService.GetIpAddress(Request);
            dbModel.AuthenticationDb.Email = email;
            dbModel.AuditDb = _auditService.UpdateAuditFields(dbModel.AuditDb, dbModel.AuthenticationDb.Email, ip);
            _authenticationUserRepository.Update(dbModel);
            return StatusCode(200, new MessageResponse("Cambiaste tu correo con exitó"));
        }

        [Authorize]
        [HttpPut("validate")]
        public IActionResult ValidateUserEmail()
        {
            var id = JwtService.GetClaimUserId(User);
            var dbModel = _authenticationUserRepository.GetById(id) as AuthenticationUserAuditModel;
            if (dbModel == null)
            {
                return StatusCode(401, "El token expiró");
            }
            JwtService.ValidateJwtVersion(dbModel.AuthenticationDb, User);
            if (!dbModel.AuthenticationDb.EmailValid)
            {
                var ip = IpService.GetIpAddress(Request);
                dbModel.AuditDb = _auditService.UpdateAuditFields(dbModel.AuditDb, dbModel.AuthenticationDb.Email, ip);
                dbModel.AuthenticationDb.EmailValid = true;
                dbModel.AuthenticationDb.VersionId = Guid.NewGuid().ToString();
                _authenticationUserRepository.Update(dbModel);
            }
            var token = JwtService.GenerateTokenWithUserId(_configuration.JwtSecret,
                DateTime.Now.AddDays(7),
                dbModel.UserDb.Id!,
                dbModel.AuthenticationDb.VersionId!);
            return StatusCode(200, new AuthenticationResponse(token));
        }
    }
}