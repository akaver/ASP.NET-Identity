using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNet.Identity;

namespace Domain.IdentityModelsIntId
{

    /// <summary>
    ///     Represents a Role entity
    /// </summary>
    public class Role : Role<int, UserRole>
    {

        /// <summary>
        ///     Constructor, set the rolename
        /// </summary>
        /// <param name="roleName"></param>
        public Role(string roleName)
        {
            Name = roleName;
        }
    }

    public class Role<TKey, TUserRole> : IRole<TKey> where TUserRole : UserRole<TKey>
    {
        public Role()
        {
            Users = new List<UserRole>();
        }

        public TKey Id { get; set; }
        [DisplayName("Role name")]
        public string Name { get; set; }
        public virtual ICollection<UserRole> Users { get; set; }
    }



}
