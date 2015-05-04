using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityBaseModels;

namespace DAL.Interfaces
{
	public interface IUserRoleRepository : IEFRepository<UserRole>
	{
	    UserRole GetByUserIdAndRoleId(object roleId, object userId);

	}
}
