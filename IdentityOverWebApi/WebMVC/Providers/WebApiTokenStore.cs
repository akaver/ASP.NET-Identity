using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace WebMVC.Providers
{
    public class WebApiTokenStore
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public Dictionary<string, TokenResponse> UserTokenStore { get; private set; }

        public WebApiTokenStore()
        {
            _logger.Debug("Created with id: " + _instanceId);
            UserTokenStore = new Dictionary<string, TokenResponse>();
        }


        public string GetInstanceId()
        {
            return _instanceId;
        }

        public TokenResponse GetTokenResponse(string userName, string password)
        {
            _logger.Debug("userName: " + userName + " password: " + password);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:30422/");


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsync("token",
                    new StringContent(
                        "grant_type=password&username=" + userName + "&password=" + password,
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded")
                    ).Result;
                _logger.Debug("Response statuscode: " + response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                   return response.Content.ReadAsAsync<TokenResponse>().Result;
                }
                else
                {
                    _logger.Debug("Reason: " + response.ReasonPhrase);
                }

            }

            return new TokenResponse();
        }

    }
}