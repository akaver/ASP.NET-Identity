using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Domain.IdentityBaseModels
{

    /// <summary>
    ///     Default EntityFramework IUser implementation
    /// </summary>
    public class User : User<string, UserLogin, UserRole, UserClaim>, IUser
    {
        /// <summary>
        ///     Constructor which creates a new Guid for the Id
        /// </summary>
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        ///     Constructor that takes a userName
        /// </summary>
        /// <param name="userName"></param>
        public User(string userName)
            : this()
        {
            UserName = userName;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authType);
            // Add custom user claims here
            return userIdentity;
        }


    }

    public class User<TKey, TUserLogin, TUserRole, TUserClaim> : IUser<TKey>
        where TUserLogin : UserLogin<TKey>
        where TUserRole : UserRole<TKey>
        where TUserClaim : UserClaim<TKey>
    {
        public User()
        {
            //SecurityStamp = Guid.NewGuid().ToString();
            this.Claims = new List<TUserClaim>();
            this.Logins = new List<TUserLogin>();
            this.Roles = new List<TUserRole>();
        }

        public TKey Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public  DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [DisplayName("User name")]
        public string UserName { get; set; }
        public virtual ICollection<TUserClaim> Claims { get; set; }
        public virtual ICollection<TUserLogin> Logins { get; set; }
        public virtual ICollection<TUserRole> Roles { get; set; }

    }
}
