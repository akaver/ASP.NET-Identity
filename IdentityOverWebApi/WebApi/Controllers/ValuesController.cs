using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using DALEF;
using Domain;

namespace WebApi.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private WebAppEFContext db = new WebAppEFContext();

        // GET: api/Values
        public IQueryable<Value> GetValues()
        {
            return db.Values;
        }

        // GET: api/Values/5
        [ResponseType(typeof(Value))]
        public IHttpActionResult GetValue(int id)
        {
            Value value = db.Values.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            return Ok(value);
        }

        // PUT: api/Values/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutValue(int id, Value value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != value.ValueId)
            {
                return BadRequest();
            }

            db.Entry(value).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValueExists(id))
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

        // POST: api/Values
        [ResponseType(typeof(Value))]
        public IHttpActionResult PostValue(Value value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Values.Add(value);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = value.ValueId }, value);
        }

        // DELETE: api/Values/5
        [ResponseType(typeof(Value))]
        public IHttpActionResult DeleteValue(int id)
        {
            Value value = db.Values.Find(id);
            if (value == null)
            {
                return NotFound();
            }

            db.Values.Remove(value);
            db.SaveChanges();

            return Ok(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValueExists(int id)
        {
            return db.Values.Count(e => e.ValueId == id) > 0;
        }
    }
}