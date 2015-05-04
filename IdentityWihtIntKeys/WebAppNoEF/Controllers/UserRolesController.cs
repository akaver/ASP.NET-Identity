using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.Interfaces;
using Domain.IdentityBaseModels;
using Microsoft.Owin.Security;
using WebAppNoEF.ViewModels;

namespace WebAppNoEF.Controllers
{
    public class UserRolesController : Controller
    {
        private WebAppEFContext db = new WebAppEFContext();

        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        private readonly IUOW _uow;
        private readonly ApplicationRoleManager _roleManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public UserRolesController(IUOW uow, ApplicationRoleManager roleManager, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        {
            _logger.Info("_instanceId: " + _instanceId);

            _uow = uow;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        // GET: UserRoles
        public ActionResult Index()
        {
            var userRoles = db.UserRoles.Include(u => u.Role).Include(u => u.User).OrderBy(a => a.Role.Name);
            return View(userRoles.ToList());
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(string userId, string roleId)
        {
            if (userId == null || roleId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole userRole = db.UserRoles.Find(userId,roleId);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            return View(userRole);
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,RoleId")] UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", userRole.RoleId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(string userId, string roleId)
        {
            if (userId == null || roleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole userRole = db.UserRoles.Find(userId, roleId);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            var vm = new EditViewModel
                     {
                         UserRole = userRole, 
                         OriginalUserRole = userRole,
                         UserSelectList = new SelectList(db.Users, "Id", "Email", userRole.UserId), 
                         RoleSelectList = new SelectList(db.Roles, "Id", "Name", userRole.RoleId)
                     };
            return View(vm);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // see on kõik väga kahtlane
                // nii ei tohiks üldse midagi muuta
                // ei tohiks korraga muuta nii rolli kui ka kasutajat

                db.Entry(vm.OriginalUserRole).State = EntityState.Deleted;
                db.UserRoles.Add(vm.UserRole);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            vm.UserSelectList = new SelectList(db.Users, "Id", "Email", vm.UserRole.UserId);
            vm.RoleSelectList = new SelectList(db.Roles, "Id", "Name", vm.UserRole.RoleId);
            return View(vm);
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(string userId, string roleId)
        {
            if (userId == null || roleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole userRole = db.UserRoles.Find(userId, roleId);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            return View(userRole);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string userId, string roleId)
        {
            UserRole userRole = db.UserRoles.Find(userId, roleId);
            db.UserRoles.Remove(userRole);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
