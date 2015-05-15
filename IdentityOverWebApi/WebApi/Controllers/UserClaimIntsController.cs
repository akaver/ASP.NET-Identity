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
    public class UserClaimsIntController : ApiController
    {
        private WebAppEFContext db = new WebAppEFContext();

        // GET: api/UserClaimsInt
        public IQueryable<UserClaimInt> GetUserClaimsInt()
        {
            return db.UserClaimsInt;
        }

        // GET: api/UserClaimsInt/5
        [ResponseType(typeof(UserClaimInt))]
        public IHttpActionResult GetUserClaimInt(int id)
        {
            UserClaimInt userClaimInt = db.UserClaimsInt.Find(id);
            if (userClaimInt == null)
            {
                return NotFound();
            }

            return Ok(userClaimInt);
        }

        // PUT: api/UserClaimsInt/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserClaimInt(int id, UserClaimInt userClaimInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userClaimInt.Id)
            {
                return BadRequest();
            }

            db.Entry(userClaimInt).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserClaimIntExists(id))
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

        // POST: api/UserClaimsInt
        [ResponseType(typeof(UserClaimInt))]
        public IHttpActionResult PostUserClaimInt(UserClaimInt userClaimInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserClaimsInt.Add(userClaimInt);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userClaimInt.Id }, userClaimInt);
        }

        // DELETE: api/UserClaimsInt/5
        [ResponseType(typeof(UserClaimInt))]
        public IHttpActionResult DeleteUserClaimInt(int id)
        {
            UserClaimInt userClaimInt = db.UserClaimsInt.Find(id);
            if (userClaimInt == null)
            {
                return NotFound();
            }

            db.UserClaimsInt.Remove(userClaimInt);
            db.SaveChanges();

            return Ok(userClaimInt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserClaimIntExists(int id)
        {
            return db.UserClaimsInt.Count(e => e.Id == id) > 0;
        }
    }
}