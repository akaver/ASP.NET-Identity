using System.Collections.Generic;
using Domain.IdentityModels;

namespace DALWebApi.Interfaces
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
