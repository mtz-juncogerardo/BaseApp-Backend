using System.Collections.Generic;
using BaseApp.Data.DbModels;

namespace BaseApp.Repositories
{
    public interface IRepositoryBehavior
    {
        IEnumerable<IDbModel> GetAll();
        IDbModel GetById(string id);

        IEnumerable<IDbModel> GetByKeyValue(string key, string value);

        public void Create(IDbModel model);

        public void Update(IDbModel model);

        public void Delete(IDbModel authUserAudit);
    }
}