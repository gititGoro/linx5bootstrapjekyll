using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Stadium5Help.Models;

namespace Stadium5Help.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult SendMessage(Message message)
		{
			message.DateReceived = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
			message.HostAddress = this.Request.UserHostAddress;
			string fileName = Path.Combine(this.HttpContext.Server.MapPath("~/App_Data/Messages"), DateTime.Now.ToString("yyyyMMddhhmmssffff") + ".txt");
			var serializer = new JavaScriptSerializer();
			System.IO.File.WriteAllText(fileName, serializer.Serialize(message));
			return new EmptyResult();
		}
	}
}