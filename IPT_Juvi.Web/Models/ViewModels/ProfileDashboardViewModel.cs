namespace IPT_Juvi.Web.Models.ViewModels;

public sealed class ProfileDashboardViewModel
{
    public string EnterpriseName { get; init; } = "";
    public string EnterpriseType { get; init; } = "";
    public string Phone { get; init; } = "";
    public string Role { get; init; } = "";

    public string ManagerName { get; init; } = "";
    public string ManagerSection { get; init; } = "";
    public string ManagerStudentId { get; init; } = "";

    public int OrdersCompleted { get; init; }
    public int ProductsListed { get; init; }
    public decimal TotalSales { get; init; }

    public decimal WalletBalance { get; init; }
    public string? PhotoPath { get; init; }
    public string? PaymentQrPath { get; init; }
}
