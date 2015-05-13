using System.Collections.Generic;
using Domain.IdentityModels;
using Microsoft.AspNet.Identity;

namespace DALWebApi.Interfaces
{
    public interface IRoleIntRepository :  IRoleRepository<int, RoleInt>
    {
    }

    public interface IRoleRepository : IRoleRepository<string, Role>
	{
	}

    public interface IRoleRepository<in TKey, TRole> : IEFRepository<TRole>
        where TRole : class, IRole<TKey>
    {
        TRole GetByRoleName(string roleName);
        List<TRole> GetRolesForUser(TKey userId);
    }

}
