using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DALWebApi.Interfaces;
using DALWebApi.Repositories;

namespace DALWebApi
{
    /// <summary>
    /// UOW, using WEB-API based repos
    /// </summary>
    public class WebApiUOW : IWebApiUOW
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();
        private string _webApiToken;

        private const string WebApiBaseUrl = "http://localhost:30422/api/";

        private readonly IDictionary<Type, Func<string, string, object>> _repositoryFactories;
        protected Dictionary<Type, object> Repositories { get; private set; }

        public WebApiUOW()
        {
            //list of factories for our repos
            _repositoryFactories = GetCustomFactories();
            //here we cache already created repos
            Repositories = new Dictionary<Type, object>();

        }

        public void SetSecurity(string securityToken)
        {
            _webApiToken = securityToken;
        }


        //TODO: Get the base URL from web.config
        public IWebApiValueRepository Values { get { return GetWebApiRepo<IWebApiValueRepository>(); } }

        // identity repos, PK - string
        public IRoleRepository Roles { get { return GetWebApiRepo<IRoleRepository>(); } }
        public IUserClaimRepository UserClaims { get { return GetWebApiRepo<IUserClaimRepository>(); } }
        public IUserLoginRepository UserLogins { get { return GetWebApiRepo<IUserLoginRepository>(); } }
        public IUserRepository Users { get { return GetWebApiRepo<IUserRepository>(); } }
        public IUserRoleRepository UserRoles { get { return GetWebApiRepo<IUserRoleRepository>(); } }

        // identity repos, PK - int
        public IRoleIntRepository RolesInt { get { return GetWebApiRepo<IRoleIntRepository>(); } }
        public IUserClaimIntRepository UserClaimsInt { get { return GetWebApiRepo<IUserClaimIntRepository>(); } }
        public IUserLoginIntRepository UserLoginsInt { get { return GetWebApiRepo<IUserLoginIntRepository>(); } }
        public IUserIntRepository UsersInt { get { return GetWebApiRepo<IUserIntRepository>(); } }
        public IUserRoleIntRepository UserRolesInt { get { return GetWebApiRepo<IUserRoleIntRepository>(); } }




        public T GetRepository<T>() where T : class
        {
            var res = GetWebApiRepo<T>();
            if (res == null)
            {
                throw new NotImplementedException("No repository for type, " + typeof(T).FullName);
            }
            return res;
        }

        // calling custom repo provier
        private T GetWebApiRepo<T>() where T : class
        {
            // Look for T dictionary cache under typeof(T).
            object repoObj;
            Repositories.TryGetValue(typeof(T), out repoObj);
            if (repoObj != null)
            {
                return (T)repoObj;
            }

            return MakeRepository<T>();
        }

        protected virtual T MakeRepository<T>()
        {
            Func<string, string, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            if (factory == null)
            {
                throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
            }

            // NB!!! _webApiBaseUrl, _webApiToken - they should be something meaningfull at this point

            var repo = (T)factory(WebApiBaseUrl, _webApiToken);
            Repositories[typeof(T)] = repo;
            return repo;
        }

        private static IDictionary<Type, Func<string, string, object>> GetCustomFactories()
        {
            return new Dictionary<Type, Func<string, string, object>>
                {
                    {typeof(IWebApiValueRepository), (baseUrl,securityToken) => new WebApiValueRepository(baseUrl+"values/", securityToken)},
                    // identity repos, PK - string
                    {typeof(IRoleRepository), (baseUrl,securityToken) => new RoleRepository(baseUrl+"Roles/", securityToken)},
                    {typeof(IUserClaimRepository), (baseUrl,securityToken) => new UserClaimRepository(baseUrl+"UserClaims/", securityToken)},
                    {typeof(IUserLoginRepository), (baseUrl,securityToken) => new UserLoginRepository(baseUrl+"UserLogins/", securityToken)},
                    {typeof(IUserRepository), (baseUrl,securityToken) => new UserRepository(baseUrl+"Users/", securityToken)},
                    {typeof(IUserRoleRepository), (baseUrl,securityToken) => new UserRoleRepository(baseUrl+"UserRoles/", securityToken)},

                    // identity repos, PK - int
                    // TODO - fix urls for different PKs
                    {typeof(IRoleIntRepository), (baseUrl,securityToken) => new RoleIntRepository(baseUrl+"RolesInt/", securityToken)},
                    {typeof(IUserClaimIntRepository), (baseUrl,securityToken) => new UserClaimIntRepository(baseUrl+"UserClaimsInt/", securityToken)},
                    {typeof(IUserLoginIntRepository), (baseUrl,securityToken) => new UserLoginIntRepository(baseUrl+"UserLoginsInt/", securityToken)},
                    {typeof(IUserIntRepository), (baseUrl,securityToken) => new UserIntRepository(baseUrl+"UsersInt/", securityToken)},
                    {typeof(IUserRoleIntRepository), (baseUrl,securityToken) => new UserRoleIntRepository(baseUrl+"UserRolesInt/", securityToken)},


                };
        }

        public void Commit()
        {
            // hmm, do nothing?
        }
    }
}
