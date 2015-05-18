using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using DALWebApi.Interfaces;
using Domain.IdentityModels;

namespace DALWebApi.Repositories
{
    public class UserClaimIntRepository :
        UserClaimRepository<int, RoleInt, UserInt, UserClaimInt, UserLoginInt, UserRoleInt>, IUserClaimIntRepository
    {
        public UserClaimIntRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }

    public class UserClaimRepository : UserClaimRepository<string, Role, User, UserClaim, UserLogin, UserRole>,
        IUserClaimRepository
    {
        public UserClaimRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }

    public class UserClaimRepository<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole> : WebApiRepository<TUserClaim>
        where TKey : IEquatable<TKey>
        where TRole : Role<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
        where TUser : User<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
        where TUserClaim : UserClaim<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
        where TUserLogin : UserLogin<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
        where TUserRole : UserRole<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public UserClaimRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }

        public List<TUserClaim> AllIncludeUser()
        {
            //return DbSet.Include(a => a.User).ToList();
            var response = WebClient.GetAsync("AllIncludeUser").Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<List<TUserClaim>>().Result;
                return res;
            }
            _logger.Error(response.ReasonPhrase);
            return new List<TUserClaim>();
        }

    }

    //public class UserClaimRepository<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole> : EFRepository<TUserClaim>
    //    where TKey : IEquatable<TKey>
    //    where TRole : Role<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUser : User<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserClaim : UserClaim<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserLogin : UserLogin<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserRole : UserRole<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //{
    //    public UserClaimRepository(IDbContext dbContext) : base(dbContext)
    //    {
    //    }

    //    public List<TUserClaim> AllIncludeUser()
    //    {
    //        return DbSet.Include(a => a.User).ToList();

    //    }

    //}
}
