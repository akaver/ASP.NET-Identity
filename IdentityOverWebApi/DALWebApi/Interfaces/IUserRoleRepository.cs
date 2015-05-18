using Domain.IdentityModels;

namespace DALWebApi.Interfaces
{
    public interface IUserRoleIntRepository : IUserRoleRepository<int, UserRoleInt>
    {

    }
    public interface IUserRoleRepository : IUserRoleRepository<string, UserRole>
    {

    }
    public interface IUserRoleRepository<in TKey, TUserRole> : IWebApiRepository<TUserRole>
        where TUserRole : class
    {
        TUserRole GetByUserIdAndRoleId(TKey roleId, TKey userId);
        void DeleteByUserIdAndRoleId(TKey roleId, TKey userId);
    }
}
