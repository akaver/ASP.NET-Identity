using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityBaseModels;
using Microsoft.AspNet.Identity;

namespace DAL.Interfaces
{
	public interface IRoleRepository<TRole, TKey, TUserRole> : IEFRepository<TRole>
        where TUserRole : UserRole<TKey>, new()
        where TRole : Role<TKey, TUserRole>, new()
	{
	    TRole GetByRoleName(string roleName);
	    List<TRole> GetRolesForUser(TKey userId);
	}
}
