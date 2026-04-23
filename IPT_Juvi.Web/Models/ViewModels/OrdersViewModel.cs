using IPT_Juvi.Web.Models.Bizzy;

namespace IPT_Juvi.Web.Models.ViewModels;

public sealed class OrdersViewModel
{
    public IReadOnlyList<OrderRecord> Orders { get; init; } = Array.Empty<OrderRecord>();
}

