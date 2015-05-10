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
        
        //get repository for type
        T GetRepository<T>() where T: class;

        // standard autocreated repos, since we do not have any special methods in interfaces
		//IEFRepository<User> Users { get; }
		//IEFRepository<Role> Roles { get; }
		//IEFRepository<UserClaim> UserClaims { get; }
		//IEFRepository<UserLogin> UserLogins { get; }


        // Identity, PK - string
        IUserRepository Users { get; }
        IUserRoleRepository UserRoles { get; }
        IRoleRepository Roles { get; }
		IUserClaimRepository UserClaims { get; }
		IUserLoginRepository UserLogins { get; }

        // Identity, PK - int
        IUserIntRepository UsersInt { get; }
        IUserRoleIntRepository UserRolesInt { get; }
        IRoleIntRepository RolesInt { get; }
        IUserClaimIntRepository UserClaimsInt { get; }
        IUserLoginIntRepository UserLoginsInt { get; }
    }
}
