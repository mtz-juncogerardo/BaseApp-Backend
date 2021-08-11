using System.Collections.Generic;
using System.Linq;
using BaseApp.Core.Helpers;
using BaseApp.Data.DataAccess;
using BaseApp.Data.DbModels;
using BaseApp.Data.DbModels.JoinedModels;

namespace BaseApp.Repositories
{
    public class AuthenticationUserRepository : IRepositoryBehavior
    {
        private readonly AuthenticationContext _context;
        public AuthenticationUserRepository(AuthenticationContext context)
        {
            _context = context;
        }

        private IEnumerable<AuthenticationUserAuditDbModel> AuthenticationUserAudit
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
                    }).Select(aua => new AuthenticationUserAuditDbModel
                {
                    AuditDb = aua.AuditFields,
                    AuthenticationDb = aua.AuthenticationUser.Authentication,
                    UserDb = aua.AuthenticationUser.User
                });
            }
        }

        public IEnumerable<IAuditDbModel> GetAll()
        {
            return AuthenticationUserAudit;
        }

        public IAuditDbModel GetById(string id)
        {
            return AuthenticationUserAudit.FirstOrDefault(r => r.UserDb.Id == id);
        }

        public IEnumerable<IAuditDbModel> GetByKeyValue(string key, string value)
        {
            return AuthenticationUserAudit.Where(r => value == (string)r.AuthenticationDb.GetType()
                .GetProperty(key)?
                .GetValue(r.AuthenticationDb));
        }

        public void Create(IAuditDbModel dbModel)
        {
            var model = dbModel as AuthenticationUserAuditDbModel;
            if (model?.AuthenticationDb == null || model.AuditDb == null)
            {
                CustomException
                    .Throw("No se puede guardar el usuario sin especificar sus parametros de autenticacion ni sus campos de Auditoria",
                        500);
                return;
            }
            _context.Authentication.Add(model.AuthenticationDb);
            _context.Audit.Add(model.AuditDb);
            _context.Users.Add(model.UserDb);
            _context.SaveChanges();
        }

        public void Update(IAuditDbModel dbModel)
        {
            var model = dbModel as AuthenticationUserAuditDbModel;
            if (model == null)
            {
                return;
            }
            _context.SaveChanges();
        }

        public void Delete(IAuditDbModel dbModel)
        {
            var model = dbModel as AuthenticationUserAuditDbModel;
            if (model == null)
            {
                return;
            }
            _context.Audit.Remove(model.AuditDb);
            _context.Authentication.Remove(model.AuthenticationDb);
            _context.SaveChanges();
        }
    }
}