using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityBaseModels;

namespace DAL.Interfaces
{
	public interface IUserClaimRepository : IEFRepository<UserClaim>
	{
	    List<UserClaim> AllIncludeUser();
	}
}
