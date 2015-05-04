using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.IdentityBaseModels;

namespace DAL.Repositories
{
	public class UserRepository : EFRepository<User>, IUserRepository
	{
		public UserRepository(IDbContext dbContext) : base(dbContext)
		{
		}

		public User GetUserByUserName(string userName)
		{
			return DbSet.FirstOrDefault(a => a.UserName.ToUpper() == userName.ToUpper());
		}

		public User GetUserByEmail(string email)
		{
			return DbSet.FirstOrDefault(a => a.Email.ToUpper() == email.ToUpper());
		}

	    public bool IsInRole(string userId, string roleName)
	    {
            return DbSet.Find(userId).Roles.Any(a => a.Role.Name == roleName);
	    }

	    public void AddUserToRole(string userId, string roleName)
	    {
	        
	    }
	}
}
