using ReceivablesAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ReceivablesAPI.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ReceivableBatch> ReceivableBatches { get; }
    DbSet<Receivable> Receivables { get; }

    DbSet<ReceivableDebtor> ReceivableDebtors { get; }
    DbSet<ReceivableDebtorAddress> ReceivableDebtorAddresses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
