using System.Threading.Tasks;
using Domain.IdentityModels;
using IdentityEF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WebApi;
using WebApi.Models;

namespace WebApi
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<UserInt,int>
    {
        public ApplicationUserManager(IUserStore<UserInt,int> store) : base(store)
        {
            // Configure validation logic for usernames
            UserValidator = new UserValidator<UserInt,int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            if (Startup.DataProtectionProvider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<UserInt,int>(Startup.DataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

    }
}
