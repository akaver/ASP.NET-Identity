using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DALWebApi;
using DALWebApi.Interfaces;
using Domain;
using Microsoft.AspNet.Identity;
using WebAppNoEF.Providers;

namespace WebAppNoEF.Controllers
{
    public class ValuesController : Controller
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        private WebAppEFContext db = new WebAppEFContext();

        private readonly IWebApiUOW _webApiUOW;
        private readonly WebApiTokenStore _webApiTokenStore;

        public ValuesController(IWebApiUOW webApiUOW, WebApiTokenStore webApiTokenStore)
        {
            _webApiUOW = webApiUOW;
            _webApiTokenStore = webApiTokenStore;


        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsAuthenticated) return;

            TokenResponse token;
            
            _webApiTokenStore.UserTokenStore.TryGetValue(User.Identity.GetUserName(), out token);

            if (token != null)
            {
                _webApiUOW.SetSecurity(token.AccessToken);
            }
        }

        // GET: Values
        public ActionResult Index()
        {
            var values = _webApiUOW.Values.All;
            return View(values);
        }

        // GET: Values/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Value value = _webApiUOW.Values.GetById(id);
            if (value == null)
            {
                return HttpNotFound();
            }
            return View(value);
        }

        // GET: Values/Create
        public ActionResult Create()
        {
            ViewBag.UserIntId = new SelectList(db.UsersInt, "Id", "Email");
            return View();
        }

        // POST: Values/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ValueId,Name,UserIntId")] Value value)
        {
            if (ModelState.IsValid)
            {
                _webApiUOW.Values.Add(value);
                return RedirectToAction("Index");
            }

            ViewBag.UserIntId = new SelectList(db.UsersInt, "Id", "Email", value.UserIntId);
            return View(value);
        }

        // GET: Values/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Value value = _webApiUOW.Values.GetById(id);
            if (value == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserIntId = new SelectList(db.UsersInt, "Id", "Email", value.UserIntId);
            return View(value);
        }

        // POST: Values/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ValueId,Name,UserIntId")] Value value)
        {
            if (ModelState.IsValid)
            {
                _webApiUOW.Values.Update(value.ValueId, value);
                return RedirectToAction("Index");
            }
            ViewBag.UserIntId = new SelectList(db.UsersInt, "Id", "Email", value.UserIntId);
            return View(value);
        }

        // GET: Values/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Value value = _webApiUOW.Values.GetById(id);
            if (value == null)
            {
                return HttpNotFound();
            }
            return View(value);
        }

        // POST: Values/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _webApiUOW.Values.Delete(id);
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
