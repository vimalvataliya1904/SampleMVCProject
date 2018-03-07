using Elaunch_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elaunch_POS.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        //[HasLoginSessionFilter]
        public ActionResult Index()
        {
            return RedirectToAction("TraderLogin", "TraderHome", new { Area = "Traders" });
        }

        [HttpPost]
        public string GetGridData()
        {
            string mode = Convert.ToString(Request.Form["mode"]);
            GridData og = new GridData(mode);
            return og.JsonData;
        }
    }
}