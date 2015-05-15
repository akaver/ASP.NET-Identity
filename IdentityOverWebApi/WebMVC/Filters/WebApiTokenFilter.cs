using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebMVC.Filters
{
    public class WebApiTokenFilter : FilterAttribute, IActionFilter
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logger.Debug(_instanceId);

            var session = filterContext.HttpContext.Session;
            var controllerName = 
                filterContext.ActionDescriptor.ControllerDescriptor.
                ControllerName.ToLower().Trim();


            _logger.Debug("IsAuthenticated:" + filterContext.HttpContext.Request.IsAuthenticated + " controllerName: " + controllerName);
            if (filterContext.HttpContext.Request.IsAuthenticated && 
                (session == null || session["token"]==null) && 
                controllerName!="account")
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "SessionLogOff"
                }));
            }

            //if (controller != null && controller.)
            //{
            //    if (session["Login"] == null)
            //    {
            //        controller.HttpContext.Response.Redirect("./Account/Login");
            //    }
            //}
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }
    }
}