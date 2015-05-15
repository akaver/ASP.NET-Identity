using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DALWebApi.Interfaces;
using Domain.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace WebMVC.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        private readonly IWebApiUOW _uow;
        private readonly ApplicationRoleManager _roleManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public RolesController(IWebApiUOW uow, ApplicationRoleManager roleManager, ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            _logger.Info("_instanceId: " + _instanceId);
            _uow = uow;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        // GET: Roles
        public ActionResult Index()
        {
            // our rolemanager does not implement IQueryableRoleStore by choice
            // so lets drop down to ef 
            //return View(_roleManager.RolesInt.ToList());

            return View(_uow.RolesInt.All.OrderBy(a => a.Name).ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int id)
        {
            if (id == default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = _roleManager.FindById(id); 
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] RoleInt role)
        {
            if (ModelState.IsValid)
            {
                _roleManager.Create(role);
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = _roleManager.FindById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] RoleInt role)
        {
            if (ModelState.IsValid)
            {
                _roleManager.Update(role);
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = _roleManager.FindById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var role = _roleManager.FindById(id);
            _roleManager.Delete(role);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _logger.Info("_instanceId: " + _instanceId+" disposing:"+disposing);
            base.Dispose(disposing);
        }
    }
}
