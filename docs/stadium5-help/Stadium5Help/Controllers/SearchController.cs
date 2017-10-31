using System.Web.Mvc;

namespace Stadium5Help.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index(string term)
        {
            return View((object)term);
        }
	}
}