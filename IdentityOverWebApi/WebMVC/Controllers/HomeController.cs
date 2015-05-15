using System;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
	public class HomeController : Controller
	{
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

		public HomeController()
		{
			_logger.Info("_instanceId: " + _instanceId);
		}

		public ActionResult Index()
		{
			return View();
		}

		protected override void Dispose(bool disposing)
		{
			_logger.Info("Disposing: " + disposing + " _instanceId: " + _instanceId);
			base.Dispose(disposing);
		}
	}
}