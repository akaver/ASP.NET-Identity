using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityBaseModels;

namespace DAL.Interfaces
{
	public interface IUserRepository : IEFRepository<User>
	{
		User GetUserByUserName(string userName);
		User GetUserByEmail(string email);
        bool IsInRole(string userId, string roleName);
        void AddUserToRole(string userId, string roleName);
	}
}
