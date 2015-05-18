using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using DALWebApi.Interfaces;
using Domain.IdentityModels;

namespace DALWebApi.Repositories
{
    public class UserIntRepository : UserRepository<int, RoleInt, UserInt, UserClaimInt, UserLoginInt, UserRoleInt>, IUserIntRepository
    {
        public UserIntRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }

    public class UserRepository : UserRepository<string, Role, User, UserClaim, UserLogin, UserRole>, IUserRepository
    {
        public UserRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }

    public class UserRepository<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole> : WebApiRepository<TUser>
        where TKey : IEquatable<TKey>
        where TRole : Role<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUser : User<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserClaim : UserClaim<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserLogin : UserLogin<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserRole : UserRole<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public UserRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }

        public TUser GetUserByUserName(string userName)
        {
            _logger.Debug(userName);
            //return DbSet.FirstOrDefault(a => a.UserName.ToUpper() == userName.ToUpper());
            var response = WebClient.GetAsync("GetUserByUserName/"+userName+"/").Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<TUser>().Result;
                return res;
            }
            _logger.Error(response.ReasonPhrase+" "+userName+" "+response.RequestMessage.RequestUri);
            return new TUser();
        }

        public TUser GetUserByEmail(string email)
        {
            _logger.Debug(email);
            //return DbSet.FirstOrDefault(a => a.Email.ToUpper() == email.ToUpper());
            var response = WebClient.GetAsync("GetUserByEmail/"+email+"/").Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<TUser>().Result;
                return res;
            }
            _logger.Error(response.ReasonPhrase);
            return new TUser();
        }

        public bool IsInRole(TKey userId, string roleName)
        {
            _logger.Debug(userId + " "+ roleName);

            //return DbSet.Find(userId).Roles.Any(a => a.Role.Name == roleName);
            var response = WebClient.GetAsync("IsInRole/" + userId.ToString()+"/"+roleName).Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<bool>().Result;
                return res;
            }
            _logger.Error(response.ReasonPhrase);
            return false;
        }

        public void AddUserToRole(TKey userId, string roleName)
        {
            _logger.Debug(userId + " " + roleName);

            var response = WebClient.GetAsync("AddUserToRole/" + userId.ToString() + "/" + roleName).Result;
            if (response.IsSuccessStatusCode)
            {
            }
            _logger.Error(response.ReasonPhrase);
        }
    }

    //public class UserRepository<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole> : EFRepository<TUser>
    //    where TKey : IEquatable<TKey>
    //    where TRole : Role<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUser : User<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserClaim : UserClaim<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserLogin : UserLogin<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserRole : UserRole<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //{
    //    public UserRepository(IDbContext dbContext)
    //        : base(dbContext)
    //    {
    //    }

    //    public TUser GetUserByUserName(string userName)
    //    {
    //        return DbSet.FirstOrDefault(a => a.UserName.ToUpper() == userName.ToUpper());
    //    }

    //    public TUser GetUserByEmail(string email)
    //    {
    //        return DbSet.FirstOrDefault(a => a.Email.ToUpper() == email.ToUpper());
    //    }

    //    public bool IsInRole(TKey userId, string roleName)
    //    {
    //        return DbSet.Find(userId).Roles.Any(a => a.Role.Name == roleName);
    //    }

    //    public void AddUserToRole(TKey userId, string roleName)
    //    {

    //    }
    //}
}
