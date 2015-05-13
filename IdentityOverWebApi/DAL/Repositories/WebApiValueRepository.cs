using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALWebApi.Interfaces;
using Domain;

namespace DALWebApi.Repositories
{
    public class WebApiValueRepository : WebApiRepository<Value>, IWebApiValueRepository
    {
        public WebApiValueRepository(string baseUrl, string securityToken) : base(baseUrl, securityToken)
        {
        }
    }
}
