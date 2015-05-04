using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityModels;

namespace DAL.Interfaces
{
	public interface IUserLoginRepository : IEFRepository<UserLogin>
	{
	    List<UserLogin> GetAllIncludeUser();
        UserLogin GetUserLoginByProviderAndProviderKey(string loginProvider, string providerKey);
	}
}
