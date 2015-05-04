using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNet.Identity;

namespace Domain.IdentityBaseModels
{

    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    public class Role : Role<string, UserRole>
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public Role()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="roleName"></param>
        public Role(string roleName)
            : this()
        {
            Name = roleName;
        }
    }

    public class Role<TKey, TUserRole> : IRole<TKey> 
        where TUserRole : UserRole<TKey>
    {
        public Role()
        {
            Users = new List<TUserRole>();
        }

        public TKey Id { get; set; }

        [DisplayName("Role name")]
        public string Name { get; set; }
        public virtual ICollection<TUserRole> Users { get; set; }
    }



}
