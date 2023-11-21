namespace ReceivablesAPI.Domain.Entities;

public class ReceivableDebtor : BaseAuditableEntity
{
    public string DebtorReference { get; set; } = string.Empty;
    public string DebtorName { get; set; } = string.Empty;

    public IList<Receivable> Receivables { get; set; } = new List<Receivable>();
}
