using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBNote.Server;
using Newtonsoft.Json;

namespace DBNote.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var menu = MenuServer.GetMenuInfo();
            ViewBag.MenuJson = JsonConvert.SerializeObject(menu);
            return View();
        }
    }
}