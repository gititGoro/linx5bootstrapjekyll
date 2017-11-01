using System.Web.Mvc;
using System.Web.Routing;

namespace Stadium5Help
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Controls",
				url: "controls/{page}",
				defaults: new { controller = "Controls", action = "Index", page = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "PageScripts",
				url: "pagescripts/{page}",
				defaults: new { controller = "PageScripts", action = "Index", page = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "ApplicationExplorer",
				url: "applicationexplorer/{page}",
				defaults: new { controller = "ApplicationExplorer", action = "Index", page = UrlParameter.Optional }
			);

            routes.MapRoute(
                name: "Releasenotes",
                url: "releasenotes/{page}",
                defaults: new { controller = "Releasenotes", action = "Index", page = UrlParameter.Optional }
            );

            routes.MapRoute(
				name: "Frameworks",
				url: "frameworks/{page}",
				defaults: new { controller = "Frameworks", action = "Index", page = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "HowItWorks",
				url: "howitworks/{page}",
				defaults: new { controller = "HowItWorks", action = "Index", page = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}