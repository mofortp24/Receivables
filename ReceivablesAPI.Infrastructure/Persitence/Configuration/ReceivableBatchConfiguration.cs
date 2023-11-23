using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReceivablesAPI.Domain.Entities;

namespace ReceivablesAPI.Infrastructure.Persitence.Configuration;
public class ReceivableBatchConfiguration : IEntityTypeConfiguration<ReceivableBatch>
{
    public void Configure(EntityTypeBuilder<ReceivableBatch> builder)
    {
        builder.HasKey(b => b.Id)
            .HasName("PK_ReceivableBatch_ReceivableBatchId");

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.ToTable("ReceivableBatch");

        builder.HasIndex(e => e.Id, "UQ_ReceivableBatch_ReceivableBatchId")
            .IsUnique();

        builder.HasIndex(e => e.BatchReference, "UQ_ReceivableBatch_Reference")
            .IsUnique();

        builder.Property(d => d.BatchReference)
            .HasMaxLength(50)
            .IsRequired();
    }
}