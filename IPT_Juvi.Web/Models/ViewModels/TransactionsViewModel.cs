using IPT_Juvi.Web.Models.Bizzy;

namespace IPT_Juvi.Web.Models.ViewModels;

public sealed class TransactionsViewModel
{
    public decimal WalletBalance { get; init; }
    public IReadOnlyList<TransactionRecord> Transactions { get; init; } = Array.Empty<TransactionRecord>();
}

