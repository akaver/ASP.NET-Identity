using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityModels;

namespace DAL.Interfaces
{
    public interface IUOW
    {
        //save pending changes to the data store
        void Commit();
        // standard autocreated repos, since we do not have any special methods in interfaces
		//IEFRepository<User> Users { get; }
		//IEFRepository<Role> Roles { get; }
		//IEFRepository<UserClaim> UserClaims { get; }
		//IEFRepository<UserLogin> UserLogins { get; }

        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IRoleRepository Roles { get; }
		IUserClaimRepository UserClaims { get; }
		IUserLoginRepository UserLogins { get; }
    }
}
