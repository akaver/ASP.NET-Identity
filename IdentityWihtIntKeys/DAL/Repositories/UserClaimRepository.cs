using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.IdentityBaseModels;

namespace DAL.Repositories
{
	public class UserClaimRepository : EFRepository<UserClaim>, IUserClaimRepository
	{
		public UserClaimRepository(IDbContext dbContext) : base(dbContext)
		{
		}

	    public List<UserClaim> AllIncludeUser()
	    {
	        return DbSet.Include(a => a.User).ToList();

	    }

	}
}
