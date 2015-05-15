using System;

namespace DALEF.Interfaces
{
    public interface IEFRepositoryProvider
    {
        IDbContext DbContext { get; set; }
        IEFRepository<T> GetRepositoryForEntityType<T>() where T : class;
        T GetRepository<T>(Func<IDbContext, object> factory = null) where T : class;
        void SetRepository<T>(T repository);
    }

}
