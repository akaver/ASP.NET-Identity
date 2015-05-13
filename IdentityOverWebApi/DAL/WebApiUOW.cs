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
        public IWebApiValueRepository Values
        {
            get
            {
                return GetWebApiRepo<IWebApiValueRepository>("https://localhost:44302/api/values/",
                    _webApiToken);
            }
        }

        // calling custom repo provier
        private T GetWebApiRepo<T>(string baseUrl, string securityToken) where T : class
        {
            // Look for T dictionary cache under typeof(T).
            object repoObj;
            Repositories.TryGetValue(typeof(T), out repoObj);
            if (repoObj != null)
            {
                return (T)repoObj;
            }

            return MakeRepository<T>(baseUrl, securityToken);
        }

        protected virtual T MakeRepository<T>(string baseUrl, string securityToken)
        {
            Func<string, string, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            if (factory == null)
            {
                throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
            }
            var repo = (T)factory(baseUrl, securityToken);
            Repositories[typeof(T)] = repo;
            return repo;
        }

        private static IDictionary<Type, Func<string, string, object>> GetCustomFactories()
        {
            return new Dictionary<Type, Func<string, string, object>>
                {
                    {typeof(IWebApiValueRepository), (baseUrl,securityToken) => new WebApiValueRepository(baseUrl, securityToken)},
                };
        }

        public void Commit()
        {
            // hmm, do nothing?
        }
    }
}
