using System;
using System.Linq;
using System.Net.Http;
using DALWebApi.Interfaces;
using Domain.IdentityModels;

namespace DALWebApi.Repositories
{
    public class UserRoleIntRepository : UserRoleRepository<int, RoleInt, UserInt, UserClaimInt, UserLoginInt, UserRoleInt>, IUserRoleIntRepository
    {
        public UserRoleIntRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }

    public class UserRoleRepository : UserRoleRepository<string, Role, User, UserClaim, UserLogin, UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }

    public class UserRoleRepository<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole> : WebApiRepository<TUserRole>
        where TKey : IEquatable<TKey>
        where TRole : Role<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUser : User<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserClaim : UserClaim<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserLogin : UserLogin<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
        where TUserRole : UserRole<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>, new()
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();


        public UserRoleRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }

        public TUserRole GetByUserIdAndRoleId(TKey roleId, TKey userId)
        {
            //return DbSet.FirstOrDefault(a => a.RoleId.Equals(roleId) && a.UserId.Equals(userId));
            //return DbSet.FirstOrDefault(a => a.Name.ToUpper() == roleName.ToUpper());
            var response = WebClient.GetAsync("GetByUserIdAndRoleId/" + roleId+"?userId="+userId).Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<TUserRole>().Result;
                return res;
            }
            _logger.Error(response.ReasonPhrase);
            return new TUserRole();
        }



        public void DeleteByUserIdAndRoleId(TKey roleId, TKey userId)
        {
            var response = WebClient.DeleteAsync("DeleteByUserIdAndRoleId/" + roleId.ToString()+"?userId="+userId.ToString()).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    //public class UserRoleRepository<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole> : EFRepository<TUserRole>
    //    where TKey : IEquatable<TKey>
    //    where TRole : Role<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUser : User<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserClaim : UserClaim<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserLogin : UserLogin<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //    where TUserRole : UserRole<TKey, TRole, TUser, TUserClaim, TUserLogin, TUserRole>
    //{
    //    public UserRoleRepository(IDbContext dbContext)
    //        : base(dbContext)
    //    {
    //    }

    //    public TUserRole GetByUserIdAndRoleId(TKey roleId, TKey userId)
    //    {
    //        return DbSet.FirstOrDefault(a => a.RoleId.Equals(roleId) && a.UserId.Equals(userId));
    //    }

    //}
}
