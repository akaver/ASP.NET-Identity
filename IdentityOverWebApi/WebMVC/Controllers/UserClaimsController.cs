using System;
using System.Net;
using System.Web.Mvc;
using DALWebApi.Interfaces;
using Domain.IdentityModels;

namespace WebMVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserClaimsController : Controller
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();
        private readonly IUOW _uow;

        public UserClaimsController(IUOW uow)
        {
            _logger.Info("_instanceId: " + _instanceId);
            _uow = uow;
        }
        // GET: UserClaims
        public ActionResult Index()
        {
            return View(_uow.GetRepository<IUserClaimIntRepository>().AllIncludeUser());
        }

        // GET: UserClaims/Details/5
        public ActionResult Details(int id)
        {
            if (id == default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userClaim = _uow.GetRepository<IUserClaimIntRepository>().GetById(id);
            if (userClaim == null)
            {
                return HttpNotFound();
            }
            return View(userClaim);
        }

        // GET: UserClaims/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email");
            return View();
        }

        // POST: UserClaims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,ClaimType,ClaimValue")] UserClaimInt userClaim)
        {
            if (ModelState.IsValid)
            {
                _uow.GetRepository<IUserClaimIntRepository>().Add(userClaim);
                _uow.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(_uow.Users.All, "Id", "Email", userClaim.UserId);
            return View(userClaim);
        }

        // GET: UserClaims/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userClaim = _uow.GetRepository<IUserClaimIntRepository>().GetById(id);
            if (userClaim == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email", userClaim.UserId);
            return View(userClaim);
        }

        // POST: UserClaims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,ClaimType,ClaimValue")] UserClaimInt userClaim)
        {
            if (ModelState.IsValid)
            {
                _uow.GetRepository<IUserClaimIntRepository>().Update(userClaim.Id, userClaim);
                _uow.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email", userClaim.UserId);
            return View(userClaim);
        }

        // GET: UserClaims/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userClaim = _uow.GetRepository<IUserClaimIntRepository>().GetById(id);
            if (userClaim == null)
            {
                return HttpNotFound();
            }
            return View(userClaim);
        }

        // POST: UserClaims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _uow.GetRepository<IUserClaimIntRepository>().Delete(id);
            _uow.Commit();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _logger.Info("_instanceId: " + _instanceId);
            base.Dispose(disposing);
        }
    }
}
