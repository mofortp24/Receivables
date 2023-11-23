using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReceivablesAPI.Domain.Entities;

namespace ReceivablesAPI.Infrastructure.Persitence.Configuration;
public class ReceivableAddressDebtorConfiguration : IEntityTypeConfiguration<ReceivableDebtorAddress>
{
    public void Configure(EntityTypeBuilder<ReceivableDebtorAddress> builder)
    {
        builder.HasKey(e => e.Id)
            .HasName("PK_ReceivableDebtorAddress_DebtorAddressId");

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.ToTable("ReceivableDebtorAddress");

        builder.Property(d => d.DebtorAddress1)
            .HasMaxLength(250);

        builder.Property(d => d.DebtorAddress2)
            .HasMaxLength(250);

        builder.Property(d => d.DebtorTown)
            .HasMaxLength(50);

        builder.Property(d => d.DebtorState)
            .HasMaxLength(50);

        builder.Property(d => d.DebtorZip)
            .HasMaxLength(10);

        builder.Property(d => d.DebtorCountryCode)
            .IsRequired();

        builder.Property(d => d.DebtorRegistrationNumber)
            .HasMaxLength(15);
    }
}