using System;
using System.Net;
using System.Web.Mvc;
using DALWebApi.Interfaces;
using Domain;

namespace WebMVC.Controllers
{
    public class ValuesController : Controller
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        private readonly IWebApiUOW _uow;

        public ValuesController(IWebApiUOW uow)
        {
            _uow = uow;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsAuthenticated) return;


            if (Session["token"] != null)
            {
                _uow.SetSecurity(Session["token"] as string);
            }
        }

        // GET: Values
        public ActionResult Index()
        {
            var values = _uow.Values.All;
            return View(values);
        }

        // GET: Values/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Value value = _uow.Values.GetById(id);
            if (value == null)
            {
                return HttpNotFound();
            }
            return View(value);
        }

        // GET: Values/Create
        public ActionResult Create()
        {
            ViewBag.UserIntId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email");
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
                _uow.Values.Add(value);
                return RedirectToAction("Index");
            }

            ViewBag.UserIntId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email", value.UserIntId);
            return View(value);
        }

        // GET: Values/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Value value = _uow.Values.GetById(id);
            if (value == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserIntId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email", value.UserIntId);
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
                _uow.Values.Update(value.ValueId, value);
                return RedirectToAction("Index");
            }
            ViewBag.UserIntId = new SelectList(_uow.GetRepository<IUserIntRepository>().All, "Id", "Email", value.UserIntId);
            return View(value);
        }

        // GET: Values/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Value value = _uow.Values.GetById(id);
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
            _uow.Values.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
