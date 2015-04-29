using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebAppNoEF
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        protected void Application_Start()
        {
            _logger.Info("_instanceId: " + _instanceId);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            _logger.Info(
                "_instanceId: " + _instanceId + 
                " URL(" + HttpContext.Current.Request.RequestType + "): " + 
                HttpContext.Current.Request.RawUrl);
        }
    }
}
