using Domain.IdentityModels;

namespace DALEF.Interfaces
{
    public interface IUserRoleIntRepository : IUserRoleRepository<int, UserRoleInt>
    {

    }
    public interface IUserRoleRepository : IUserRoleRepository<string, UserRole>
    {

    }
    public interface IUserRoleRepository<in TKey, TUserRole> : IEFRepository<TUserRole>
        where TUserRole : class
    {
        TUserRole GetByUserIdAndRoleId(TKey roleId, TKey userId);

    }
}
