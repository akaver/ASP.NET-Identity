using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using DALEF;
using DALEF.Interfaces;
using Domain.IdentityModels;

namespace WebApi.Controllers
{
    //TODO: write controller based on generics, implementing both pks
    public class UsersIntController : ApiController
    {
        //private WebAppEFContext db = new WebAppEFContext();
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        private readonly IUOW _uow;
        public UsersIntController(IUOW uow)
        {
            _logger.Debug(_instanceId);
            _uow = uow;

        }
        #region Custom methods

        // use trailing slash on route - or iis will throw 404 on email address
        [Route("api/UsersInt/GetUserByUserName/{userName}/")]
        [HttpGet]
        [ResponseType(typeof(UserInt))]
        public IHttpActionResult GetUserByUserName(string userName)
        {
            _logger.Debug(_instanceId);
            var userInt = _uow.GetRepository<IUserIntRepository>().GetUserByUserName(userName);
            if (userInt == null)
            {
                _logger.Info("NotFound: "+userName);
                return NotFound();
            }
            _logger.Debug("Found: " + userName);

            return Ok(userInt);
        }

        [Route("api/UsersInt/GetUserByEmail/{userEmail}/")]
        [HttpGet]
        [ResponseType(typeof(UserInt))]
        public IHttpActionResult GetUserByEmail(string userEmail)
        {
            var userInt = _uow.GetRepository<IUserIntRepository>().GetUserByEmail(userEmail);
            if (userInt == null)
            {
                return NotFound();
            }

            return Ok(userInt);
        }

        [Route("api/UsersInt/IsInRole/{userId}/{roleName}")]
        [HttpGet]
        [ResponseType(typeof(bool))]
        public IHttpActionResult IsInRole(int userId, string roleName)
        {
            var isInRole = _uow.GetRepository<IUserIntRepository>().IsInRole(userId,roleName);
            return Ok(isInRole);
        }

        [Route("AddUserToRole")]
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddUserToRole(int userId, string roleName)
        {
            //TODO
            _uow.GetRepository<IUserIntRepository>().AddUserToRole(userId, roleName);
            return Ok();
        }
        #endregion


        #region standard CRUD
        // GET: api/UsersInt
        public ICollection<UserInt> GetUsersInt()
        {
            return _uow.GetRepository<IUserIntRepository>().All;
        }

        // GET: api/UsersInt/5
        [ResponseType(typeof(UserInt))]
        public IHttpActionResult GetUserInt(int id)
        {
            var userInt = _uow.GetRepository<IUserIntRepository>().GetById(id);
            if (userInt == null)
            {
                return NotFound();
            }

            return Ok(userInt);
        }

        // PUT: api/UsersInt/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserInt(int id, UserInt userInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userInt.Id)
            {
                return BadRequest();
            }

            _uow.GetRepository<IUserIntRepository>().Update(userInt);
            try
            {
                _uow.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserIntExists(id))
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

        // POST: api/UsersInt
        [ResponseType(typeof(UserInt))]
        public IHttpActionResult PostUserInt(UserInt userInt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _uow.GetRepository<IUserIntRepository>().Add(userInt);
            _uow.Commit();

            _logger.Debug(userInt.Email+" ("+userInt.Id+")");
            return CreatedAtRoute("DefaultApi", new { id = userInt.Id }, userInt);
        }

        // DELETE: api/UsersInt/5
        [ResponseType(typeof(UserInt))]
        public IHttpActionResult DeleteUserInt(int id)
        {
            var userInt = _uow.GetRepository<IUserIntRepository>().GetById(id);
            if (userInt == null)
            {
                return NotFound();
            }

            _uow.GetRepository<IUserIntRepository>().Delete(userInt);
            _uow.Commit();

            return Ok(userInt);
        }
        #endregion

        #region helpers
        private bool UserIntExists(int id)
        {
            //TODO: here should be repo method, since .All gives full list from db, possibly millions of records
            return _uow.GetRepository<IUserIntRepository>().All.Count(e => e.Id == id) > 0;
        }
        #endregion

    }
}