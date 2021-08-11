using System;
using BaseApp.Core.Helpers;
using BaseApp.Data.DataAccess;

namespace BaseApp.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly AuthenticationContext _context;
        
        public RepositoryFactory(AuthenticationContext context)
        {
            _context = context;
        }

        public IRepositoryBehavior Choose(Type repository)
        {
            if (repository == typeof(AuthenticationUserRepository))
            {
                return new AuthenticationUserRepository(_context);
            }
            if (repository == typeof(ArticleRepository))
            {
                return new ArticleRepository(_context);
            }
            CustomException.Throw("El repositorio no existe", 500);
            return null;
        }
    }
}