using System;
using System.Web.Mvc;

namespace Stadium5Help.Controllers
{
    public class ApplicationExplorerController : Controller
    {
		public ActionResult Index(string page)
		{
			if (String.IsNullOrEmpty(page))
			{
				return View();
			}
			else
			{
				ViewBag.Title = page;
				return View(page);
			}
		}
	}
}