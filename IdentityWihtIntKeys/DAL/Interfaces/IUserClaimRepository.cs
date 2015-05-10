using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityModels;

namespace DAL.Interfaces
{
    public interface IUserClaimIntRepository : IUserClaimRepository<UserClaimInt>
    {
    }
    public interface IUserClaimRepository : IUserClaimRepository<UserClaim>
    {
    }
    public interface IUserClaimRepository<TUserClaim> : IEFRepository<TUserClaim>
        where TUserClaim : class
    {
        List<TUserClaim> AllIncludeUser();
    }
}
