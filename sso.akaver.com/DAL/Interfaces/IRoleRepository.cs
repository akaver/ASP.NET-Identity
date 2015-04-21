using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityModels;

namespace DAL.Interfaces
{
	public interface IRoleRepository : IEFRepository<Role>
	{
	    Role GetByRoleName(string roleName);
	    List<Role> GetRolesForUser(string userId);
	}
}
