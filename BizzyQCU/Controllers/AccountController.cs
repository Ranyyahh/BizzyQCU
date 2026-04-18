using Microsoft.AspNetCore.Mvc;
using BizzyQCU.Models;

namespace BizzyQCU.Controllers
{
    public class AccountController : Controller
    {
        // GET: LOGIN PAGE (exactly like PPT Page 3 & 7)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: LOGIN
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // TEMPORARY: Replace with database later
            if (username == "test" && password == "123")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }

        // GET: CUSTOMER REGISTRATION (exactly like PPT Page 4 & 7)
        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            return View();
        }

        // POST: CUSTOMER REGISTRATION
        [HttpPost]
        public IActionResult RegisterCustomer(Customer model, IFormFile qcuIdFile)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save to database
                // TODO: Save uploaded file
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // GET: ENTERPRISE REGISTRATION (exactly like PPT Page 5 & 8)
        [HttpGet]
        public IActionResult RegisterEnterprise()
        {
            return View();
        }

        // POST: ENTERPRISE REGISTRATION
        [HttpPost]
        public IActionResult RegisterEnterprise(Enterprise model, IFormFile documentFile)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save to database
                // TODO: Save uploaded file
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}