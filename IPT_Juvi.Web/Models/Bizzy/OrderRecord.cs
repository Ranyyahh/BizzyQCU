namespace IPT_Juvi.Web.Models.Bizzy;

public sealed class OrderRecord
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public DateTimeOffset OrderedAt { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Completed";
}

