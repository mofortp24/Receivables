namespace ReceivablesAPI.Domain.Entities;

public class ReceivableBatch : BaseAuditableEntity
{
    public string? BatchReference { get; set; }

    public IList<Receivable> Receivables { get; private set; } = new List<Receivable>();
}
