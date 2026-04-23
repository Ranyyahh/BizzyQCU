using System.ComponentModel.DataAnnotations;

namespace IPT_Juvi.Web.Models.ViewModels;

public sealed class AddFundsViewModel
{
    [Required]
    [Range(typeof(decimal), "1", "9999999")]
    public decimal Amount { get; set; } = 500;
}

