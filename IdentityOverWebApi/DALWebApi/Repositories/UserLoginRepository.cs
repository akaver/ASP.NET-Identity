using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using DALWebApi.Interfaces;
using Domain.IdentityModels;

namespace DALWebApi.Repositories
{
    public class UserLoginIntRepository : UserLoginRepository<int, RoleInt, UserInt, UserClaimInt, UserLoginInt, UserRoleInt>, IUserLoginIntRepository
    {
        public UserLoginIntRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }

    public class UserLoginRepository : UserLoginRepository<string, Role, User, UserClaim, UserLogin, UserRole>, IUserLoginRepository
    {
        public UserLoginRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }

    public class UserLoginRepository<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole> : WebApiRepository<TUserLogin>
        where TKey : IEquatable<TKey>
        where TRole : Role<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUser : User<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserClaim : UserClaim<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserLogin : UserLogin<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserRole : UserRole<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public UserLoginRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }

        public List<TUserLogin> GetAllIncludeUser()
        {
            //return DbSet.Include(a => a.User).ToList();

            var response = WebClient.GetAsync("GetAllIncludeUser").Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<List<TUserLogin>>().Result;
                return res;
            }
            _logger.Error(response.ReasonPhrase);
            return new List<TUserLogin>();
        }

        public TUserLogin GetUserLoginByProviderAndProviderKey(string loginProvider, string providerKey)
        {
            //return DbSet.FirstOrDefault(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
            var response = WebClient.GetAsync("GetUserLoginByProviderAndProviderKey/" + loginProvider + "?providerKey=" + providerKey).Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<TUserLogin>().Result;
                return res;
            }
            _logger.Error(response.ReasonPhrase);
            return new TUserLogin();
        }

    }

    //public class UserLoginRepository<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole> : EFRepository<TUserLogin>
    //    where TKey : IEquatable<TKey>
    //    where TRole : Role<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUser : User<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserClaim : UserClaim<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserLogin : UserLogin<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserRole : UserRole<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //{
    //    public UserLoginRepository(IDbContext dbContext)
    //        : base(dbContext)
    //    {
    //    }

    //    public List<TUserLogin> GetAllIncludeUser()
    //    {
    //        return DbSet.Include(a => a.User).ToList();
    //    }

    //    public TUserLogin GetUserLoginByProviderAndProviderKey(string loginProvider, string providerKey)
    //    {
    //        return DbSet.FirstOrDefault(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
    //    }

    //}
}
