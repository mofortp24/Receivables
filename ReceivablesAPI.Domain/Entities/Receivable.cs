namespace ReceivablesAPI.Domain.Entities;

public class Receivable : BaseAuditableEntity
{
    public string Reference { get; set; }
    public CurrencyCode CurrencyCode { get; set; }
    public DateTime? IssueDate { get; set; }
    public decimal OpeningValue { get; set; }
    public decimal PaidValue { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public bool? Cancelled { get; set; }

    public ReceivableDebtor? Debtor { get; set; }

    public ReceivableDebtorAddress? DebtorAddress { get; set; }

    
}