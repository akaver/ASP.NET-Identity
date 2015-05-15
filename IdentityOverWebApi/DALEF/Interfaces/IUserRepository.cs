using Domain.IdentityModels;
using Microsoft.AspNet.Identity;

namespace DALEF.Interfaces
{
    public interface IUserIntRepository : IUserRepository<int, UserInt>
    {
    }

    public interface IUserRepository : IUserRepository<string, User>
    {
    }

    public interface IUserRepository<in TKey, TUser> : IEFRepository<TUser>
        where TUser : class, IUser<TKey>
    {
		TUser GetUserByUserName(string userName);
		TUser GetUserByEmail(string email);
        bool IsInRole(TKey userId, string roleName);
        void AddUserToRole(TKey userId, string roleName);
	}
}
