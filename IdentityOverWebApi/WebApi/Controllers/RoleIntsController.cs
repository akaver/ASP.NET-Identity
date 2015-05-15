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
    public class RolesIntController : ApiController
    {
        private WebAppEFContext db = new WebAppEFContext();

        // GET: api/RolesInt
        public IQueryable<RoleInt> GetRolesInt()
        {
            return db.RolesInt;
        }

        // GET: api/RolesInt/5
        [ResponseType(typeof(RoleInt))]
        public IHttpActionResult GetRoleInt(int id)
        {
            RoleInt roleInt = db.RolesInt.Find(id);
            if (roleInt == null)
            {
                return NotFound();
            }

            return Ok(roleInt);
        }

        // PUT: api/RolesInt/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRoleInt(int id, RoleInt roleInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roleInt.Id)
            {
                return BadRequest();
            }

            db.Entry(roleInt).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleIntExists(id))
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

        // POST: api/RolesInt
        [ResponseType(typeof(RoleInt))]
        public IHttpActionResult PostRoleInt(RoleInt roleInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RolesInt.Add(roleInt);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = roleInt.Id }, roleInt);
        }

        // DELETE: api/RolesInt/5
        [ResponseType(typeof(RoleInt))]
        public IHttpActionResult DeleteRoleInt(int id)
        {
            RoleInt roleInt = db.RolesInt.Find(id);
            if (roleInt == null)
            {
                return NotFound();
            }

            db.RolesInt.Remove(roleInt);
            db.SaveChanges();

            return Ok(roleInt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoleIntExists(int id)
        {
            return db.RolesInt.Count(e => e.Id == id) > 0;
        }
    }
}