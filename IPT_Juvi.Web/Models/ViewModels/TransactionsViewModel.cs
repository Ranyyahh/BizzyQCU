namespace IPT_Juvi.Web.Models.ViewModels;

public sealed class TransactionsViewModel
{
    public string? Query { get; init; }
    public IReadOnlyList<IPT_Juvi.Web.Models.Bizzy.SalesTransactionRow> Rows { get; init; } =
        Array.Empty<IPT_Juvi.Web.Models.Bizzy.SalesTransactionRow>();
}
