namespace IPT_Juvi.Web.Models.Bizzy;

public sealed class EnterpriseProfile
{
    public string EnterpriseName { get; set; } = "";
    public string EnterpriseType { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";

    public string? PhotoPath { get; set; }
}

