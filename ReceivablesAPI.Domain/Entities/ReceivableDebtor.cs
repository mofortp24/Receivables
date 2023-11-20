namespace ReceivablesAPI.Domain.Entities;

public class ReceivableDebtor : BaseAuditableEntity
{
    //public int DebtorId { get; set; }

    public int ReceivableId { get; set; }

    public string DebtorReference { get; set; }

    public string DebtorName { get; set; }

    public ReceivableDebtorAddress DebtorAddress { get; set; }

    public virtual Receivable Receivable { get; set; }

    //public virtual ReceivableDebtorAddress DebtorAddress { get; set; }
}
