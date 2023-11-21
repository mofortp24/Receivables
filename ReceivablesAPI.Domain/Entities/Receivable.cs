namespace ReceivablesAPI.Domain.Entities;

public class Receivable : BaseAuditableEntity
{
    //public int ReceivableId { get; set; }

    //public string BatchReference { get; set; }
    public int ReceivableBatchId { get; set; }

    public string Reference { get; set; } = string.Empty;
    public CurrencyCode CurrencyCode { get; set; }
    public DateTime? IssueDate { get; set; }
    public decimal OpeningValue { get; set; }
    public decimal PaidValue { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool? Cancelled { get; set; }

    public int DebtorId { get; set; }

    public int DebtorAddressId { get; set; }

    //public ReceivableDebtor Debtor { get; set; } = new ();
    public virtual ReceivableDebtor Debtor { get; set; } = null!;

    public virtual ReceivableDebtorAddress DebtorAddress { get; set; } = null!;

    public virtual ReceivableBatch Batch { get; set; } = null!;


}