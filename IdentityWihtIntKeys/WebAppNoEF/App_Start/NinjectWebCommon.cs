using System;
using System.Web;
using DAL;
using DAL.Helpers;
using DAL.Interfaces;
using Domain.IdentityBaseModels;
using Domain.IdentityModels;
using Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using WebAppNoEF;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace WebAppNoEF
{
	public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDbContext>().To<WebAppEFContext>().InRequestScope();
            //kernel.Bind<IDbContextFactory>().To<DbContextFactory>().InRequestScope();

            kernel.Bind<EFRepositoryFactories>().To<EFRepositoryFactories>().InSingletonScope();
			kernel.Bind<IEFRepositoryProvider>().To<EFRepositoryProvider>().InRequestScope();
			kernel.Bind<IUOW>().To<UOW>().InRequestScope();

	        kernel.Bind<IUserStore<CustomUser, int>>().To<CustomUserStore>();
            kernel.Bind<IRoleStore<CustomRole, int>>().To<CustomRoleStore>();

	        kernel.Bind<ApplicationSignInManager>().To<ApplicationSignInManager>();
	        kernel.Bind<ApplicationUserManager>().To<ApplicationUserManager>();
            kernel.Bind<ApplicationRoleManager>().To<ApplicationRoleManager>();

	        kernel.Bind<IAuthenticationManager>().ToMethod(a => HttpContext.Current.GetOwinContext().Authentication);
        }        
    }
}
