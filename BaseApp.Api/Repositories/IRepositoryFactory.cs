using System;

namespace BaseApp.Repositories
{
    public interface IRepositoryFactory
    {
        IRepositoryBehavior Choose(Type repository);
    }
}