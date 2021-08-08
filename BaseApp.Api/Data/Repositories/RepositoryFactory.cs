using System;
using BaseApp.Core.Helpers;
using BaseApp.Data.DataAccess;
using BaseApp.Data.Responses;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BaseApp.Data.Repositories
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
            if (repository == typeof(UserRepository))
            {
                return new UserRepository(_context);
            }
            CustomException.Throw("El repositorio no existe", 500);
            return null;
        }
    }
}