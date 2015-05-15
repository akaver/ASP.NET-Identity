using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DALWebApi.Interfaces;

namespace DALWebApi.Repositories
{
    public class WebApiRepository<T> : IWebApiRepository<T>, IDisposable
        where T : class
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        protected HttpClient WebClient = new HttpClient();

        public WebApiRepository(string baseUrl, string securityToken)
        {
            _logger.Debug(baseUrl);

            //https://localhost:44302/api/<somecontroller>/
            // initialize web api client
            // get the token????
            WebClient.BaseAddress = new Uri(baseUrl);
            WebClient.DefaultRequestHeaders.Accept.Clear();
            WebClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!String.IsNullOrWhiteSpace(securityToken))
            {
                WebClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", securityToken);
            }


        }

        public List<T> All
        {
            get
            {
                _logger.Debug("");

                var response = WebClient.GetAsync("").Result;
                if (response.IsSuccessStatusCode)
                {
                    var res = response.Content.ReadAsAsync<List<T>>().Result;
                    return res;
                }
                _logger.Error(response.ReasonPhrase + " All " + WebClient.BaseAddress + " GetAsync "+typeof(T).FullName);
                return new List<T>();
            }
        }

        public T GetById(object id)
        {
            _logger.Debug(id);

            var response = WebClient.GetAsync(id.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<T>().Result;
                return res;
            }
            _logger.Error(response.ReasonPhrase + " GetById " + WebClient.BaseAddress+id.ToString() + " GetAsync " + typeof(T).FullName);
            return null;
        }

        public void Add(T entity)
        {
            _logger.Debug(entity);

            var response = WebClient.PostAsJsonAsync("", entity).Result;
            if (!response.IsSuccessStatusCode)
            {
                _logger.Fatal(response.ReasonPhrase + " PostAsJsonAsync " + WebClient.BaseAddress + typeof(T).FullName);
                throw new Exception(response.ReasonPhrase + " " + WebClient.BaseAddress + " PostAsJsonAsync " + typeof(T).FullName);
            }
            entity = response.Content.ReadAsAsync<T>().Result;
            _logger.Debug(response.Headers.Location);
        }

        public void Update(object id, T entity)
        {
            _logger.Debug(id);

            var response = WebClient.PutAsJsonAsync(id.ToString(), entity).Result;
            if (!response.IsSuccessStatusCode)
            {
                _logger.Fatal(response.ReasonPhrase + " PutAsJsonAsync " + WebClient.BaseAddress + id.ToString() + typeof(T).FullName);
                throw new Exception(response.ReasonPhrase + " " + WebClient.BaseAddress + id.ToString() + " PutAsJsonAsync " + typeof(T).FullName);
            }
        }


        public void Delete(object id)
        {
            _logger.Debug(id);

            var response = WebClient.DeleteAsync(id.ToString()).Result;
            if (!response.IsSuccessStatusCode)
            {
                _logger.Fatal(response.ReasonPhrase + " DeleteAsync " + WebClient.BaseAddress + id.ToString() + typeof(T).FullName);
                throw new Exception(response.ReasonPhrase + " " + WebClient.BaseAddress + id.ToString() + " DeleteAsync " + typeof(T).FullName);
            }
        }

        public void Dispose()
        {
            WebClient.Dispose();
        }
    }
}
