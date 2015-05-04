using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Helpers;
using DAL.Interfaces;
using Newtonsoft.Json;
using Ninject;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();

            //var kernel = new StandardKernel();
            //            kernel.Bind<IDbContext>().To<WebAppEFContext>();
            ////kernel.Bind<IDbContextFactory>().To<DbContextFactory>().InRequestScope();

            //kernel.Bind<EFRepositoryFactories>().To<EFRepositoryFactories>();
            //kernel.Bind<IEFRepositoryProvider>().To<EFRepositoryProvider>();
            //kernel.Bind<IUOW>().To<UOW>();
            //IUOW _uow = kernel.Get<IUOW>();

            //var ctx = new WebAppEFContext();
            //var userId = ctx.Users.FirstOrDefault(a => a.Email == "akaver@akaver.com").Id;
            //var roleId = ctx.Roles.FirstOrDefault(a => a.Name == "admin").Id;

            //Console.WriteLine(userId + " " + roleId);

            //ctx.UserRoles.Add(new UserRole() { RoleId = roleId, UserId = userId });
            //ctx.SaveChanges();


            //var roleName = ctx.Roles.FirstOrDefault(a => a.Name == "admin").Name;
            //roleName = "kore";
            //var res = ctx.Users.Find(userId).Roles.Any(a => a.Role.Name == roleName);


        }

        static async Task RunAsync()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44302/");


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync("token",
                    new StringContent(
                        "grant_type=password&username=api@akaver.com&password=Kala.Maja1",
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded")
                        );
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsAsync<TokenResponse>();
                    Console.WriteLine("{0}\t${1}\t{2}", token.AccessToken, token.TokenType, token.UserName);
                    Console.WriteLine("--------------------");

                    client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", token.AccessToken);
                    response = await client.GetAsync("api/values/");
                    if (response.IsSuccessStatusCode)
                    {
                        var res = await response.Content.ReadAsAsync<List<string>>();
                        foreach (var s in res)
                        {
                            Console.WriteLine(s);
                        }

                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                    }                
                }
                else
                {
                    Console.WriteLine(response.StatusCode + " " + response.ReasonPhrase);
                }

                
            }
        }


    }

    public class TokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }
        
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
    }
}
