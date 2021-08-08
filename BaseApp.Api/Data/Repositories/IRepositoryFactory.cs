using System;

namespace BaseApp.Data.Repositories
{
    public interface IRepositoryFactory
    {
        IRepositoryBehavior Choose(Type repository);
    }
}