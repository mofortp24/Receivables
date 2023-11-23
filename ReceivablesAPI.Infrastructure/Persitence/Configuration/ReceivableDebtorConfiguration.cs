using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReceivablesAPI.Domain.Entities;

namespace ReceivablesAPI.Infrastructure.Persitence.Configuration;
public class ReceivableDebtorConfiguration : IEntityTypeConfiguration<ReceivableDebtor>
{
    public void Configure(EntityTypeBuilder<ReceivableDebtor> builder)
    {
        builder.HasKey(e => e.Id)
            .HasName("PK_ReceivableDebtor_DebtorId");

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.ToTable("ReceivableDebtor");

        builder.Property(d => d.DebtorName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.DebtorReference)
            .HasMaxLength(50)
            .IsRequired();
    }
}