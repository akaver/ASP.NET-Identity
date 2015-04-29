using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.Interfaces;
using Domain.IdentityModels;

namespace DAL.Repositories
{
	public class UserLoginRepository : EFRepository<UserLogin>, IUserLoginRepository
	{
		public UserLoginRepository(IDbContext dbContext) : base(dbContext)
		{
		}

        public List<UserLogin> GetAllIncludeUser()
        {
            return DbSet.Include(a => a.User).ToList();
        }

	    public UserLogin GetUserLoginByProviderAndProviderKey(string loginProvider, string providerKey)
	    {
            return DbSet.FirstOrDefault(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
	    }

	}
}
