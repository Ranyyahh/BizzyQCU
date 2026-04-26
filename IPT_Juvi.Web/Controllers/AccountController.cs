using Microsoft.AspNetCore.Mvc;
using IPT_Juvi.Web.Models.ViewModels;
using IPT_Juvi.Web.Services;

namespace IPT_Juvi.Web.Controllers;

public sealed class AccountController : Controller
{
    private readonly InMemoryBizzyStore _store;
    private readonly IWebHostEnvironment _env;

    public AccountController(InMemoryBizzyStore store, IWebHostEnvironment env)
    {
        _store = store;
        _env = env;
    }

    [HttpGet]
    public IActionResult Profile()
    {
        return View(_store.GetDashboard());
    }

    [HttpGet]
    public IActionResult Settings()
    {
        return View(_store.GetSettings());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(ProfileSettingsViewModel model, IFormFile? photo, IFormFile? paymentQr, string? submit)
    {
        if (string.Equals(submit, "photo", StringComparison.OrdinalIgnoreCase) && photo is not null && photo.Length > 0)
        {
            var ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (ext is not (".png" or ".jpg" or ".jpeg" or ".gif" or ".webp"))
            {
                TempData["ToastError"] = "Unsupported file type.";
                return RedirectToAction(nameof(Settings));
            }

            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await photo.CopyToAsync(stream);
            }

            _store.UpdatePhotoPath($"/uploads/{fileName}");
            TempData["ToastSuccess"] = "Photo updated.";
            return RedirectToAction(nameof(Settings));
        }

        if (string.Equals(submit, "qr", StringComparison.OrdinalIgnoreCase) && paymentQr is not null && paymentQr.Length > 0)
        {
            var ext = Path.GetExtension(paymentQr.FileName).ToLowerInvariant();
            if (ext is not (".png" or ".jpg" or ".jpeg" or ".gif" or ".webp"))
            {
                TempData["ToastError"] = "Unsupported file type.";
                return RedirectToAction(nameof(Settings));
            }

            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"payment-qr-{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await paymentQr.CopyToAsync(stream);
            }

            _store.UpdatePaymentQrPath($"/uploads/{fileName}");
            TempData["ToastSuccess"] = "GCash QR updated.";
            return RedirectToAction(nameof(Settings));
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _store.SaveSettings(model);
        return RedirectToAction(nameof(Profile));
    }

    [HttpGet]
    public IActionResult Wallet()
    {
        return View(_store.GetWallet());
    }

    [HttpGet]
    public IActionResult AddFunds()
    {
        return View(new AddFundsViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddFunds(AddFundsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _store.AddFunds(model.Amount);
        TempData["ToastSuccess"] = "Funds added.";
        return RedirectToAction(nameof(Wallet));
    }

    [HttpGet]
    public IActionResult Transactions(string? q)
    {
        return View(_store.GetTransactions(q));
    }

    [HttpGet]
    public IActionResult Orders()
    {
        return View(_store.GetOrders());
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!_store.ChangePassword(model.CurrentPassword, model.NewPassword))
        {
            ModelState.AddModelError(nameof(ChangePasswordViewModel.CurrentPassword), "Current password is incorrect.");
            return View(model);
        }

        TempData["ToastSuccess"] = "Password updated.";
        return RedirectToAction(nameof(Settings));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        TempData["ToastSuccess"] = "Logged out.";
        return RedirectToAction("Index", "Home");
    }
}
