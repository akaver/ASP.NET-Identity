using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.IdentityBaseModels;

namespace DAL.Repositories
{
	public class UserRoleRepository : EFRepository<UserRole>, IUserRoleRepository
	{
        public UserRoleRepository(IDbContext dbContext)
            : base(dbContext)
		{
		}

	    public UserRole GetByUserIdAndRoleId(object roleId, object userId)
	    {
	        return DbSet.FirstOrDefault(a => a.RoleId == roleId && a.UserId == userId);
	    }

	}
}
