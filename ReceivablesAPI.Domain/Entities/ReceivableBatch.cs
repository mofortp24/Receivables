﻿namespace ReceivablesAPI.Domain.Entities;

public class ReceivableBatch : BaseAuditableEntity
{
    //public int ReceivableBatchId { get; set; }

    public string BatchReference { get; set; } = string.Empty;

    public IList<Receivable> Receivables { get; private set; } = new List<Receivable>();
}
