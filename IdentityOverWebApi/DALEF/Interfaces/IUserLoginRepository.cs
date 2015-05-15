using System.Collections.Generic;
using Domain.IdentityModels;

namespace DALEF.Interfaces
{
    public interface IUserLoginIntRepository : IUserLoginRepository<UserLoginInt>
    {
    }
    public interface IUserLoginRepository : IUserLoginRepository<UserLogin>
    {
    }
    public interface IUserLoginRepository<TUserLogin> : IEFRepository<TUserLogin>
        where TUserLogin : class 
    {
        List<TUserLogin> GetAllIncludeUser();
        TUserLogin GetUserLoginByProviderAndProviderKey(string loginProvider, string providerKey);
    }
}
