using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User_Profile_Seller_Juvi.Controllers
{
    // ==================== MODELS ====================
    [Serializable]
    public class EnterpriseSummary
    {
        public string PhotoDataUrl { get; set; }
        public string QrDataUrl { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Gcash { get; set; }
        public string Role { get; set; }
    }

    [Serializable]
    public class ManagerSummary
    {
        public string Name { get; set; }
        public string Section { get; set; }
        public string StudentId { get; set; }
        public string ContactNumber { get; set; }
    }

    [Serializable]
    public class EnterpriseStatsSummary
    {
        public int OrdersCompleted { get; set; }
        public int ProductsListed { get; set; }
        public decimal TotalSales { get; set; }
    }

    [Serializable]
    public class EnterpriseDashboardViewModel
    {
        public EnterpriseSummary Enterprise { get; set; }
        public ManagerSummary Manager { get; set; }
        public EnterpriseStatsSummary Stats { get; set; }
    }

    [Serializable]
    public class UserProfileViewModel
    {
        public string PhotoDataUrl { get; set; }
        public string QrDataUrl { get; set; }

        [Required(ErrorMessage = "Enterprise name is required.")]
        [Display(Name = "Enterprise Name")]
        public string EnterpriseName { get; set; }

        [Required(ErrorMessage = "Enterprise type is required.")]
        [Display(Name = "Enterprise Type")]
        public string EnterpriseType { get; set; }

        [Required(ErrorMessage = "Gcash number is required.")]
        [Display(Name = "Gcash No.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Enter an 11-digit Gcash number starting with 09.")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Manager name is required.")]
        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; }

        [Required(ErrorMessage = "Student ID is required.")]
        [Display(Name = "Student ID")]
        [RegularExpression(@"^\d{2}-\d{4}$", ErrorMessage = "Use the format 00-0000.")]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "Section is required.")]
        [Display(Name = "Section")]
        public string Section { get; set; }

        [Required(ErrorMessage = "Manager Gcash number is required.")]
        [Display(Name = "Manager Gcash No.")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Enter an 11-digit Gcash number starting with 09.")]
        public string ManagerContactNumber { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "New password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm the new password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "New password and confirmation do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountSettingsPageViewModel
    {
        public UserProfileViewModel Profile { get; set; }
        public ChangePasswordViewModel PasswordChange { get; set; }
        public bool ReopenPasswordModal { get; set; }
    }

    [Serializable]
    public class TransactionRecordViewModel
    {
        public string UserName { get; set; }
        public string Product { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
    }

    public class TransactionsPageViewModel
    {
        public string SearchTerm { get; set; }
        public IList<TransactionRecordViewModel> Transactions { get; set; }
    }

    // ==================== HOME CONTROLLER ====================
    public class HomeController : Controller
    {
        private const string ProfileSessionKey = "ProfileState";
        private const string PasswordSessionKey = "ProfilePassword";

        public ActionResult Index()
        {
            ViewBag.PageClass = "page-dashboard";
            ViewBag.ShowBackButton = true;
            ViewBag.NavSection = "home";
            return View(BuildDashboardViewModel());
        }

        // ==================== CUSTOMER/USER PROFILE ====================
        // URL: /Home/Profile
        public ActionResult Profile()
        {
            var model = new AccountSettingsPageViewModel
            {
                Profile = new UserProfileViewModel
                {
                    PhotoDataUrl = DefaultPhotoDataUrl(),
                    EnterpriseName = "Juan Dela Cruz",
                    Email = "juandelacruz@email.com",
                    Contact = "09123456789",
                    EnterpriseType = "Customer",
                    ManagerName = "",
                    StudentId = "",
                    Section = "",
                    ManagerContactNumber = "",
                    QrDataUrl = ""
                },
                PasswordChange = new ChangePasswordViewModel()
            };
            return View("UserProfile", model);
        }

        // ==================== ENTERPRISE PROFILE ====================
        // URL: /Home/EnterpriseProfile (may sariling view na EnterpriseProfile.cshtml)
        public ActionResult EnterpriseProfile()
        {
            PrepareProfilePage("EnterpriseProfile", "enterprises");
            return View("EnterpriseProfile", BuildAccountSettingsViewModel(GetProfile(), new ChangePasswordViewModel()));
        }

        // ==================== ACCOUNT SETTINGS (iba pa to) ====================
        public ActionResult AccountSettings()
        {
            PrepareProfilePage("AccountSettings", "none");
            return View("Profile", BuildAccountSettingsViewModel(GetProfile(), new ChangePasswordViewModel()));
        }

        // ==================== REDIRECTS (for backward compatibility) ====================
        public ActionResult UserProfile()
        {
            return RedirectToAction("Profile");
        }

        // ==================== SAVE METHODS ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEnterpriseProfile([Bind(Prefix = "Profile")] UserProfileViewModel model)
        {
            PrepareProfilePage("EnterpriseProfile", "enterprises");

            if (string.IsNullOrWhiteSpace(model.PhotoDataUrl))
            {
                model.PhotoDataUrl = DefaultPhotoDataUrl();
            }

            if (!ModelState.IsValid)
            {
                return View("EnterpriseProfile", BuildAccountSettingsViewModel(model, new ChangePasswordViewModel()));
            }

            Session[ProfileSessionKey] = model;
            TempData["FlashMessage"] = "Enterprise profile information saved.";
            return RedirectToAction("EnterpriseProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProfile([Bind(Prefix = "Profile")] UserProfileViewModel model)
        {
            TempData["FlashMessage"] = "User profile saved successfully.";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAccountSettings([Bind(Prefix = "Profile")] UserProfileViewModel model)
        {
            PrepareProfilePage("AccountSettings", "none");

            if (string.IsNullOrWhiteSpace(model.PhotoDataUrl))
            {
                model.PhotoDataUrl = DefaultPhotoDataUrl();
            }

            if (!ModelState.IsValid)
            {
                return View("Profile", BuildAccountSettingsViewModel(model, new ChangePasswordViewModel()));
            }

            Session[ProfileSessionKey] = model;
            TempData["FlashMessage"] = "Profile information saved.";
            return RedirectToAction("AccountSettings");
        }

        // ==================== PASSWORD METHODS ====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Prefix = "PasswordChange")] ChangePasswordViewModel model)
        {
            PrepareProfilePage("EnterpriseProfile", "enterprises");

            var profile = GetProfile();
            var pageModel = BuildAccountSettingsViewModel(profile, model);
            pageModel.ReopenPasswordModal = true;

            if (!ModelState.IsValid)
            {
                return View("EnterpriseProfile", pageModel);
            }

            if (string.Equals(model.CurrentPassword, model.NewPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("PasswordChange.NewPassword", "New password must be different from the current password.");
            }

            if (!ModelState.IsValid)
            {
                return View("EnterpriseProfile", pageModel);
            }

            TempData["FlashMessage"] = "Demo mode: password change validated successfully.";
            return RedirectToAction("EnterpriseProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordDemo([Bind(Prefix = "PasswordChange")] ChangePasswordViewModel model)
        {
            PrepareProfilePage("AccountSettings", "none");

            var profile = GetProfile();
            var pageModel = BuildAccountSettingsViewModel(profile, model);
            pageModel.ReopenPasswordModal = true;

            if (!ModelState.IsValid)
            {
                return View("Profile", pageModel);
            }

            if (string.Equals(model.CurrentPassword, model.NewPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("PasswordChange.NewPassword", "New password must be different from the current password.");
            }

            if (!ModelState.IsValid)
            {
                return View("Profile", pageModel);
            }

            TempData["FlashMessage"] = "Demo mode: password change validated successfully.";
            return RedirectToAction("AccountSettings");
        }

        // ==================== OTHER ACTIONS ====================
        public ActionResult Transactions(string search = null)
        {
            ViewBag.PageClass = "page-transactions";
            ViewBag.HideGlobalHeader = true;
            ViewBag.NavSection = "none";

            var records = GetTransactions();
            if (!string.IsNullOrWhiteSpace(search))
            {
                records = records
                    .Where(record =>
                        record.UserName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        record.Product.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            return View(new TransactionsPageViewModel
            {
                SearchTerm = search ?? string.Empty,
                Transactions = records
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Clear();
            TempData["FlashMessage"] = "You have been logged out.";
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.PageClass = "page-about";
            ViewBag.ShowBackButton = true;
            ViewBag.NavSection = "about";
            return View();
        }

        public ActionResult Contact()
        {
            return RedirectToAction("Transactions");
        }

        // ==================== PRIVATE METHODS ====================
        private void PrepareProfilePage(string settingsAction, string navSection)
        {
            ViewBag.PageClass = "page-profile";
            ViewBag.ShowBackButton = true;
            ViewBag.SettingsAction = settingsAction;
            ViewBag.NavSection = navSection;
        }

        private AccountSettingsPageViewModel BuildAccountSettingsViewModel(UserProfileViewModel profile, ChangePasswordViewModel passwordChange)
        {
            return new AccountSettingsPageViewModel
            {
                Profile = profile,
                PasswordChange = passwordChange ?? new ChangePasswordViewModel()
            };
        }

        private EnterpriseDashboardViewModel BuildDashboardViewModel()
        {
            var profile = GetProfile();

            return new EnterpriseDashboardViewModel
            {
                Enterprise = new EnterpriseSummary
                {
                    PhotoDataUrl = profile.PhotoDataUrl,
                    QrDataUrl = profile.QrDataUrl,
                    Name = profile.EnterpriseName,
                    Type = profile.EnterpriseType == "Partnership" ? "Food" : profile.EnterpriseType,
                    Gcash = profile.Contact,
                    Role = "Innovation Strategist"
                },
                Manager = new ManagerSummary
                {
                    Name = profile.ManagerName,
                    Section = profile.Section,
                    StudentId = profile.StudentId,
                    ContactNumber = profile.ManagerContactNumber
                },
                Stats = new EnterpriseStatsSummary
                {
                    OrdersCompleted = 120,
                    ProductsListed = 35,
                    TotalSales = 50000m
                }
            };
        }

        private UserProfileViewModel GetProfile()
        {
            var profile = Session[ProfileSessionKey] as UserProfileViewModel;
            if (profile != null)
            {
                return profile;
            }

            profile = new UserProfileViewModel
            {
                PhotoDataUrl = DefaultPhotoDataUrl(),
                QrDataUrl = string.Empty,
                EnterpriseName = "Magic Powder",
                EnterpriseType = "Partnership",
                Contact = "09561234567",
                Email = "PabluEzkubr@gmail.com",
                ManagerName = "Pablo Escobar",
                StudentId = "24-6769",
                Section = "SBENT-3D",
                ManagerContactNumber = "09561234567"
            };

            Session[ProfileSessionKey] = profile;
            Session[PasswordSessionKey] = "bizzyqcu123";

            return profile;
        }

        private List<TransactionRecordViewModel> GetTransactions()
        {
            return new List<TransactionRecordViewModel>
            {
                new TransactionRecordViewModel
                {
                    UserName = "Rick Grimes",
                    Product = "2x Pork Sisig Rice, 1x Mountain Dew",
                    Date = new DateTime(2026, 4, 25),
                    Total = 700m
                },
                new TransactionRecordViewModel
                {
                    UserName = "Boy Abunda",
                    Product = "1x Caramel Macchiato, 1x Tuna Sandwich",
                    Date = new DateTime(2026, 4, 22),
                    Total = 1000m
                },
                new TransactionRecordViewModel
                {
                    UserName = "Carl Poppa",
                    Product = "4pcs Pork Siomai, 1x Gulaman",
                    Date = new DateTime(2026, 4, 15),
                    Total = 300m
                },
                new TransactionRecordViewModel
                {
                    UserName = "Raja Kulambu",
                    Product = "1x Beef Pares w/ Rice",
                    Date = new DateTime(2026, 3, 29),
                    Total = 500m
                }
            };
        }

        private string DefaultPhotoDataUrl()
        {
            return "data:image/svg+xml;utf8," + HttpUtility.UrlEncode(
                "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 160 160'>" +
                "<rect width='160' height='160' rx='80' fill='#efefef'/>" +
                "<circle cx='80' cy='58' r='28' fill='#c9c9c9'/>" +
                "<path d='M39 136c7-24 23-38 41-38s34 14 41 38' fill='#c9c9c9'/>" +
                "</svg>");
        }
    }
}