using System.Collections.Generic;
using System.Linq;
using BaseApp.Core.Helpers;
using BaseApp.Data.DataAccess;
using BaseApp.Data.DbModels;
using BaseApp.Data.Models;

namespace BaseApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticationContext _context;
        public UserRepository(AuthenticationContext context)
        {
            _context = context;
        }

        public IEnumerable<AuthenticationUserAuditModel> All
        {
            get
            {
                return _context.Authentication.Join(_context.Users,
                    a => a.Id,
                    u => u.AuthenticationDbModelId,
                    (a, u) => new
                    {
                        Authentication = a,
                        User = u
                    }).Join(_context.Audit,
                    au => au.User.AuditDbModelId,
                    af => af.Id,
                    (au, af) => new
                    {
                        AuthenticationUser = au,
                        AuditFields = af
                    }).Select(aua => new AuthenticationUserAuditModel
                {
                    AuditDb = aua.AuditFields,
                    AuthenticationDb = aua.AuthenticationUser.Authentication,
                    UserDb = aua.AuthenticationUser.User
                });
            }
        }

        public AuthenticationUserAuditModel GetById(string id)
        {
            return All.FirstOrDefault(r => r.UserDb.Id == id);
        }

        public AuthenticationUserAuditModel GetByEmail(string email)
        {
            return All.FirstOrDefault(r => r.AuthenticationDb.Email == email);
        }

        public void Create(UserDbModel user)
        {
            if (user.AuthenticationDbModel == null || user.AuditDbModel == null)
            {
                CustomException
                    .Throw("No se puede guardar el usuario sin especificar sus parametros de autenticacion ni sus campos de Auditoria",
                        500);
            }
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(AuthenticationUserAuditModel authUserAudit)
        {
            _context.Users.Update(authUserAudit.UserDb);
            _context.Audit.Update(authUserAudit.AuditDb);
            _context.Authentication.Update(authUserAudit.AuthenticationDb);
            _context.SaveChanges();
        }

        public void Delete(AuthenticationUserAuditModel authUserAudit)
        {
            _context.Audit.Remove(authUserAudit.AuditDb);
            _context.Authentication.Remove(authUserAudit.AuthenticationDb);
        }
    }
}