using System.Web.Mvc;
using WebMVC.Filters;

namespace WebMVC
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
            filters.Add(new WebApiTokenFilter());
		}
	}
}
