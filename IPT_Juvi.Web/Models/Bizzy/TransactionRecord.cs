namespace IPT_Juvi.Web.Models.Bizzy;

public sealed class TransactionRecord
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Type { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
}

