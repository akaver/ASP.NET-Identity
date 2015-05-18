using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityModels;
using Microsoft.AspNet.Identity;

namespace IdentityWebApi
{
    public class ClaimsIdentityFactory : ClaimsIdentityFactory<UserInt, int>
    {
        public override async Task<ClaimsIdentity> CreateAsync(UserManager<UserInt, int> manager, UserInt user,
            string authenticationType)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");
            if ((object)user == null)
                throw new ArgumentNullException("user");

            if (user.Id == 0) // happens after create new user, when user is created over web-api (no proxy objects, so cant get back id from database)
                user = await manager.FindByNameAsync(user.UserName); // userName must be unique for this to work (or we could use email)

            var id = new ClaimsIdentity(authenticationType, this.UserNameClaimType, this.RoleClaimType);
            id.AddClaim(new Claim(this.UserIdClaimType, this.ConvertIdToString(user.Id), "http://www.w3.org/2001/XMLSchema#string"));
            id.AddClaim(new Claim(this.UserNameClaimType, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
            id.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));

            if (manager.SupportsUserRole)
            {
                var roles = await manager.GetRolesAsync(user.Id).ConfigureAwait(false);
                foreach (var str in (IEnumerable<string>)roles)
                    id.AddClaim(new Claim(this.RoleClaimType, str, "http://www.w3.org/2001/XMLSchema#string"));
            }

            if (manager.SupportsUserClaim)
                id.AddClaims((IEnumerable<Claim>)await manager.GetClaimsAsync(user.Id).ConfigureAwait(false));

            return id;
        }

    }

}
