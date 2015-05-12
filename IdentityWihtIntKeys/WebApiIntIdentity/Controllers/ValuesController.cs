using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAL.Interfaces;

namespace WebApiApp.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        private readonly IUOW _uow;

        public ValuesController(IUOW uow)
        {
            _uow = uow;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return _uow.Users.All.Select(a=>a.Email);
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
