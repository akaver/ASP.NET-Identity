using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALWebApi.Interfaces
{
    public interface IWebApiUOW
    {
        //save pending changes to the data store
        void Commit();


        void SetSecurity(string securityToken);

        //get repository for type
        T GetRepository<T>() where T : class;

        // dummy repo for testing
        IWebApiValueRepository Values { get; }

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
