using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Interfaces;
using Domain.IdentityBaseModels;

namespace WebAppNoEF.Controllers
{
    [Authorize]
    public class UserLoginsController : Controller
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();
        private readonly IUOW _uow;

        public UserLoginsController(IUOW uow)
        {
            _logger.Info("_instanceId: " + _instanceId);
            _uow = uow;
        }

        // GET: UserLogins
        public ActionResult Index()
        {
            return View(_uow.UserLogins.GetAllIncludeUser());
        }

        // GET: UserLogins/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLogin userLogin = _uow.UserLogins.GetById(id);
            if (userLogin == null)
            {
                return HttpNotFound();
            }
            return View(userLogin);
        }

        // GET: UserLogins/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(_uow.Users.All, "Id", "Email");
            return View();
        }

        // POST: UserLogins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoginProvider,ProviderKey,UserId")] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                _uow.UserLogins.Add(userLogin);
                _uow.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(_uow.Users.All, "Id", "Email", userLogin.UserId);
            return View(userLogin);
        }

        // GET: UserLogins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLogin userLogin = _uow.UserLogins.GetById(id);
            if (userLogin == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(_uow.Users.All, "Id", "Email", userLogin.UserId);
            return View(userLogin);
        }

        // POST: UserLogins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoginProvider,ProviderKey,UserId")] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                _uow.UserLogins.Update(userLogin);
                _uow.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(_uow.Users.All, "Id", "Email", userLogin.UserId);
            return View(userLogin);
        }

        // GET: UserLogins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLogin userLogin = _uow.UserLogins.GetById(id);
            if (userLogin == null)
            {
                return HttpNotFound();
            }
            return View(userLogin);
        }

        // POST: UserLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            _uow.UserLogins.Delete(id);
            _uow.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _logger.Info("_instanceId: " + _instanceId+" disposing:"+disposing);
            base.Dispose(disposing);
        }
    }
}
