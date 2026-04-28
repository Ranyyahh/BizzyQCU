using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzyQCU.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult AddProduct()
        {
                return View();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Tracking()
        {
            return View();
        }

        public ActionResult History()
        {
            return View();
        }

        public ActionResult Manage()
        {
            return View();
        }
    } // This one closes the HomeController class
} // This one closes the Namespace