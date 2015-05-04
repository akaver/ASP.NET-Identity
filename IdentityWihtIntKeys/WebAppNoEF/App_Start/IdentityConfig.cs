using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using DAL;
using DAL.Helpers;
using Domain.IdentityBaseModels;
using Domain.IdentityModels;
using Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace WebAppNoEF
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<CustomUser, int>
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public ApplicationUserManager(IUserStore<CustomUser, int> store)
            : base(store)
        {
			_logger.Info("_instanceId: " + _instanceId);

			// Configure validation logic for usernames
			UserValidator = new UserValidator<CustomUser, int>(this)
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

			// Configure user lockout defaults
			UserLockoutEnabledByDefault = true;
			DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			MaxFailedAccessAttemptsBeforeLockout = 5;

			// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
			// You can write your own provider and plug it in here.
			RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<CustomUser, int>
			{
				MessageFormat = "Your security code is {0}"
			});
			RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<CustomUser, int>
			{
				Subject = "Security Code",
				BodyFormat = "Your security code is {0}"
			});
			//EmailService = new EmailService();
			//SmsService = new SmsService();
			if (Startup.DataProtectionProvider != null)
			{
				UserTokenProvider =
					new DataProtectorTokenProvider<CustomUser, int>(Startup.DataProtectionProvider.Create("ASP.NET Identity"));
			}
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<CustomUser, int>
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
			_logger.Info("_instanceId: " + _instanceId);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(CustomUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

		//public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
		//{
		//	return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
		//}


    }

    public class ApplicationRoleManager : RoleManager<CustomRole, int>
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public ApplicationRoleManager(IRoleStore<CustomRole, int> store) : base(store)
        {
            _logger.Info("_instanceId: " + _instanceId);
            RoleValidator = new RoleValidator<CustomRole, int>(this);
        }
    }

}
