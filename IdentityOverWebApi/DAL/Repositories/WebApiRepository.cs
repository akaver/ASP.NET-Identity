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
        protected HttpClient WebClient = new HttpClient();

        public WebApiRepository(string baseUrl, string securityToken)
        {
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
                var response = WebClient.GetAsync("").Result;
                if (response.IsSuccessStatusCode)
                {
                    var res = response.Content.ReadAsAsync<List<T>>().Result;
                    return res;
                }
                return new List<T>();
            }
        }

        public T GetById(object id)
        {
            var response = WebClient.GetAsync(id.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                var res = response.Content.ReadAsAsync<T>().Result;
                return res;
            }
            return null;
        }

        public void Add(T entity)
        {
            var response = WebClient.PostAsJsonAsync("", entity).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public void Update(object id, T entity)
        {
            var response = WebClient.PutAsJsonAsync(id.ToString(), entity).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }


        public void Delete(object id)
        {
            var response = WebClient.DeleteAsync(id.ToString()).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public void Dispose()
        {
            WebClient.Dispose();
        }
    }
}
