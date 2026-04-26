using System.ComponentModel.DataAnnotations;

namespace IPT_Juvi.Web.Models.ViewModels;

public sealed class ProfileSettingsViewModel
{
    public string? PhotoPath { get; init; }
    public string? PaymentQrPath { get; init; }

    [Required]
    public string EnterpriseName { get; set; } = "";

    [Required]
    public string EnterpriseType { get; set; } = "";

    [Required]
    public string Contact { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string ManagerName { get; set; } = "";

    [Required]
    public string StudentId { get; set; } = "";

    [Required]
    public string Section { get; set; } = "";

    [Required]
    public string ManagerContact { get; set; } = "";

    [Required]
    [EmailAddress]
    public string ManagerEmail { get; set; } = "";
}
