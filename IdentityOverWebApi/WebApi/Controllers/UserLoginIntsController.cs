using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using DALEF;
using Domain.IdentityModels;

namespace WebApi.Controllers
{
    public class UserLoginsIntController : ApiController
    {
        private WebAppEFContext db = new WebAppEFContext();

        // GET: api/UserLoginsInt
        public IQueryable<UserLoginInt> GetUserLoginsInt()
        {
            return db.UserLoginsInt;
        }

        // GET: api/UserLoginsInt/5
        [ResponseType(typeof(UserLoginInt))]
        public IHttpActionResult GetUserLoginInt(string id)
        {
            UserLoginInt userLoginInt = db.UserLoginsInt.Find(id);
            if (userLoginInt == null)
            {
                return NotFound();
            }

            return Ok(userLoginInt);
        }

        // PUT: api/UserLoginsInt/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserLoginInt(string id, UserLoginInt userLoginInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userLoginInt.LoginProvider)
            {
                return BadRequest();
            }

            db.Entry(userLoginInt).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLoginIntExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UserLoginsInt
        [ResponseType(typeof(UserLoginInt))]
        public IHttpActionResult PostUserLoginInt(UserLoginInt userLoginInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserLoginsInt.Add(userLoginInt);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserLoginIntExists(userLoginInt.LoginProvider))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userLoginInt.LoginProvider }, userLoginInt);
        }

        // DELETE: api/UserLoginsInt/5
        [ResponseType(typeof(UserLoginInt))]
        public IHttpActionResult DeleteUserLoginInt(string id)
        {
            UserLoginInt userLoginInt = db.UserLoginsInt.Find(id);
            if (userLoginInt == null)
            {
                return NotFound();
            }

            db.UserLoginsInt.Remove(userLoginInt);
            db.SaveChanges();

            return Ok(userLoginInt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserLoginIntExists(string id)
        {
            return db.UserLoginsInt.Count(e => e.LoginProvider == id) > 0;
        }
    }
}