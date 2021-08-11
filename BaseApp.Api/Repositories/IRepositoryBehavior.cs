using System.Collections.Generic;
using BaseApp.Data.DbModels;

namespace BaseApp.Repositories
{
    public interface IRepositoryBehavior
    {
        IEnumerable<IAuditDbModel> GetAll();
        IAuditDbModel GetById(string id);

        IEnumerable<IAuditDbModel> GetByKeyValue(string key, string value);

        public void Create(IAuditDbModel model);

        public void Update(IAuditDbModel model);

        public void Delete(IAuditDbModel model);
    }
}