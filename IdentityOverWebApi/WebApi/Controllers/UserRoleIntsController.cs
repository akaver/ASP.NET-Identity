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
    public class UserRolesIntController : ApiController
    {
        private WebAppEFContext db = new WebAppEFContext();

        // GET: api/UserRolesInt
        public IQueryable<UserRoleInt> GetUserRolesInt()
        {
            return db.UserRolesInt;
        }

        // GET: api/UserRolesInt/5
        [ResponseType(typeof(UserRoleInt))]
        public IHttpActionResult GetUserRoleInt(int id)
        {
            UserRoleInt userRoleInt = db.UserRolesInt.Find(id);
            if (userRoleInt == null)
            {
                return NotFound();
            }

            return Ok(userRoleInt);
        }

        // PUT: api/UserRolesInt/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserRoleInt(int id, UserRoleInt userRoleInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userRoleInt.UserId)
            {
                return BadRequest();
            }

            db.Entry(userRoleInt).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleIntExists(id))
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

        // POST: api/UserRolesInt
        [ResponseType(typeof(UserRoleInt))]
        public IHttpActionResult PostUserRoleInt(UserRoleInt userRoleInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserRolesInt.Add(userRoleInt);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserRoleIntExists(userRoleInt.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userRoleInt.UserId }, userRoleInt);
        }

        // DELETE: api/UserRolesInt/5
        [ResponseType(typeof(UserRoleInt))]
        public IHttpActionResult DeleteUserRoleInt(int id)
        {
            UserRoleInt userRoleInt = db.UserRolesInt.Find(id);
            if (userRoleInt == null)
            {
                return NotFound();
            }

            db.UserRolesInt.Remove(userRoleInt);
            db.SaveChanges();

            return Ok(userRoleInt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserRoleIntExists(int id)
        {
            return db.UserRolesInt.Count(e => e.UserId == id) > 0;
        }
    }
}