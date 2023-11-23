using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReceivablesAPI.Domain.Entities;

namespace ReceivablesAPI.Infrastructure.Persitence.Configuration;
public class ReceivableConfiguration : IEntityTypeConfiguration<Receivable>
{
    public void Configure(EntityTypeBuilder<Receivable> builder)
    {
        builder.HasKey(e => e.Id)
            .HasName("PK_Receivable_ReceivableId");

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.ToTable("Receivable");

        builder.HasOne(d => d.Batch)
            .WithMany(p => p.Receivables)
            .HasForeignKey(d => d.ReceivableBatchId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(d => d.Debtor)
            .WithMany(p => p.Receivables)
            .HasForeignKey(d => d.DebtorId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasOne(d => d.DebtorAddress)
            .WithMany(p => p.Receivables)
            .HasForeignKey(d => d.DebtorAddressId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Property(e => e.OpeningValue)
            .HasColumnType("decimal")
            .HasPrecision(18,2);
        builder.Property(e => e.PaidValue)
            .HasColumnType("decimal")
            .HasPrecision(18,2);

        builder.Property(d => d.Reference)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.CurrencyCode)
            .IsRequired();

        builder.Property(d => d.IssueDate)
            .IsRequired();

        builder.Property(d => d.DueDate)
            .IsRequired();
    }
}