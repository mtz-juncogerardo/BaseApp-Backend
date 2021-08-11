using System.Collections.Generic;
using System.Linq;
using BaseApp.Core.Helpers;
using BaseApp.Data.DataAccess;
using BaseApp.Data.DbModels;
using BaseApp.Data.DbModels.JoinedModels;

namespace BaseApp.Repositories
{
    public class ArticleRepository : IRepositoryBehavior
    {
        private readonly AuthenticationContext _context;
        
        public ArticleRepository(AuthenticationContext context)
        {
            _context = context;
        }

        public IEnumerable<ArticleAuditDbModel> ArticleAudit
        {
            get
            {
                return _context.Article.Join(_context.Audit,
                    a => a.AuditDbModelId,
                    au => au.Id,
                    (a, au) => new
                    {
                        Article = a,
                        Audit = au
                    }).Select(aau => new ArticleAuditDbModel
                {
                    ArticleDb = aau.Article,
                    AuditDb = aau.Audit
                });
            }
        }

        public IEnumerable<IAuditDbModel> GetAll()
        {
            return ArticleAudit.ToList();
        }

        public IAuditDbModel GetById(string id)
        {
            return ArticleAudit.FirstOrDefault(r => r.ArticleDb.Id == id);
        }

        public IEnumerable<IAuditDbModel> GetByKeyValue(string key, string value)
        {
            return ArticleAudit.Where(r => value == (string)r.ArticleDb.GetType().GetProperty(key)?.GetValue(r.ArticleDb));
        }

        public void Create(IAuditDbModel model)
        {
            var dbModel = model as ArticleAuditDbModel;
            if (dbModel?.ArticleDb == null || dbModel.AuditDb == null)
            {
                CustomException.Throw("No se especifico un articulo o parametros de auditoria a guardar", 500);
                return;
            }
            _context.Audit.Add(dbModel.AuditDb);
            _context.Article.Add(dbModel.ArticleDb);
            _context.SaveChanges();
        }

        public void Update(IAuditDbModel model)
        {
            var dbModel = model as ArticleAuditDbModel;
            if (dbModel == null)
            {
                return;
            }
            _context.SaveChanges();
        }

        public void Delete(IAuditDbModel model)
        {
            var dbModel = model as ArticleAuditDbModel;
            if (dbModel == null)
            {
                return;
            }
            _context.Audit.Remove(dbModel.AuditDb);
            _context.SaveChanges();
        }
    }
}