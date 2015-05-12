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
using Domain.IdentityModels;
using Microsoft.Owin.Security;
using WebAppNoEF.ViewModels;

namespace WebAppNoEF.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly WebAppEFContext _db = new WebAppEFContext();

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
            var userRoles = _db.UserRolesInt.Include(u => u.Role).Include(u => u.User).OrderBy(a => a.Role.Name);
            return View(userRoles.ToList());
        }

        // GET: UserRoles/Details/5
        public ActionResult Details(int userId, int roleId)
        {
            if (userId == default(int) || roleId==default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userRole = _db.UserRolesInt.Find(userId,roleId);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            return View(userRole);
        }

        // GET: UserRoles/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(_uow.GetRepository<IRoleIntRepository>().All, "Id", "Name");
            ViewBag.UserId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,RoleId")] UserRoleInt userRole)
        {
            if (ModelState.IsValid)
            {
                _uow.GetRepository<IUserRoleIntRepository>().Add(userRole);
                _uow.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(_uow.GetRepository<IRoleIntRepository>().All, "Id", "Name", userRole.RoleId);
            ViewBag.UserId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRoles/Edit/5
        public ActionResult Edit(int userId, int roleId)
        {
            if (userId == default(int) || roleId == default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userRole = _db.UserRolesInt.Find(userId, roleId);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            var vm = new EditViewModel
                     {
                         UserRole = userRole, 
                         OriginalUserRole = userRole,
                         UserSelectList = new SelectList(_db.Users, "Id", "Email", userRole.UserId), 
                         RoleSelectList = new SelectList(_db.Roles, "Id", "Name", userRole.RoleId)
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

                _db.Entry(vm.OriginalUserRole).State = EntityState.Deleted;
                _db.UserRolesInt.Add(vm.UserRole);

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            vm.UserSelectList = new SelectList(_db.Users, "Id", "Email", vm.UserRole.UserId);
            vm.RoleSelectList = new SelectList(_db.Roles, "Id", "Name", vm.UserRole.RoleId);
            return View(vm);
        }

        // GET: UserRoles/Delete/5
        public ActionResult Delete(int userId, int roleId)
        {
            if (userId == default(int) || roleId == default(int))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userRole = _db.UserRolesInt.Find(userId, roleId);
            if (userRole == null)
            {
                return HttpNotFound();
            }
            return View(userRole);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int userId, int roleId)
        {
            var userRole = _db.UserRolesInt.Find(userId, roleId);
            _db.UserRolesInt.Remove(userRole);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
