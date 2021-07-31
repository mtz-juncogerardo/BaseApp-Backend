using System.Collections.Generic;
using BaseApp.Data.DbModels;
using BaseApp.Data.Models;

namespace BaseApp.Data.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<AuthenticationUserAuditModel> All { get; }

        AuthenticationUserAuditModel GetById(string id);

        AuthenticationUserAuditModel GetByEmail(string email);

        public void Create(UserDbModel user);

        public void Update(AuthenticationUserAuditModel authUserAudit);

        public void Delete(AuthenticationUserAuditModel authUserAudit);
    }
}