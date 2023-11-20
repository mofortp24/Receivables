namespace ReceivablesAPI.Domain.Entities;

public class Receivable : BaseAuditableEntity
{
    //public int ReceivableId { get; set; }

    //public string BatchReference { get; set; }
    public int ReceivableBatchId { get; set; }

    public string Reference { get; set; }
    public CurrencyCode CurrencyCode { get; set; }
    public DateTime? IssueDate { get; set; }
    public decimal OpeningValue { get; set; }
    public decimal PaidValue { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool? Cancelled { get; set; }

    public ReceivableDebtor Debtor { get; set; }

    //public ReceivableDebtorAddress DebtorAddress { get; set; }

    public virtual ReceivableBatch Batch { get; set; }

    
}