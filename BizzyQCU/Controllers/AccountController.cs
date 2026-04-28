using System.Web;
using System.Web.Mvc;
using BizzyQCU.Models;

namespace BizzyQCU.Controllers
{
    public class AccountController : Controller
    {
        // LOGIN
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username == "test" && password == "123")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // REGISTER CUSTOMER
        public ActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterCustomer(Customer model, HttpPostedFileBase qcuIdFile)
        {
            if (ModelState.IsValid)
            {
                if (qcuIdFile != null && qcuIdFile.ContentLength > 0)
                {
                    string path = Server.MapPath("~/Uploads/" + qcuIdFile.FileName);
                    qcuIdFile.SaveAs(path);
                }

                return RedirectToAction("Login");
            }
            return View(model);
        }

        // REGISTER ENTERPRISE
        public ActionResult RegisterEnterprise()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterEnterprise(Enterprise model, HttpPostedFileBase documentFile)
        {
            if (ModelState.IsValid)
            {
                if (documentFile != null && documentFile.ContentLength > 0)
                {
                    string path = Server.MapPath("~/Uploads/" + documentFile.FileName);
                    documentFile.SaveAs(path);
                }

                return RedirectToAction("Login");
            }
            return View(model);
        }
    }
}
