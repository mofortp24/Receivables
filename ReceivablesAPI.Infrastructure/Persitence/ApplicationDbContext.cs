using System.Reflection;
using Duende.IdentityServer.EntityFramework.Options;
using ReceivablesAPI.Application.Common.Interfaces;
using ReceivablesAPI.Domain.Entities;
using ReceivablesAPI.Infrastructure.Identity;
using ReceivablesAPI.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static IdentityModel.ClaimComparer;
using System;

namespace ReceivablesAPI.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options, null)
    {
    }

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) 
        : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<ReceivableBatch> ReceivableBatches => Set<ReceivableBatch>();
    public DbSet<Receivable> Receivables => Set<Receivable>();
    public DbSet<ReceivableDebtor> ReceivableDebtors => Set<ReceivableDebtor>();
    public DbSet<ReceivableDebtorAddress> ReceivableDebtorAddresses => Set<ReceivableDebtorAddress>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<ReceivableBatch>(entity =>
        {
            entity.HasKey(b => b.Id)
                .HasName("PK_ReceivableBatch_ReceivableBatchId");

            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.ToTable("ReceivableBatch");

            entity.HasIndex(e => e.Id, "UQ_ReceivableBatch_ReceivableBatchId")
                .IsUnique();

            entity.HasIndex(e => e.BatchReference, "UQ_ReceivableBatch_Reference")
                .IsUnique();
        });

        builder.Entity<Receivable>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_Receivable_ReceivableId");

            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.ToTable("Receivable");

            entity.HasOne(d => d.Batch)
                .WithMany(p => p.Receivables)
                .HasForeignKey(d => d.ReceivableBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(e => e.OpeningValue)
                .HasColumnType("decimal")
                .HasPrecision(18,2);
            entity.Property(e => e.PaidValue)
                .HasColumnType("decimal")
                .HasPrecision(18,2);

        });

        builder.Entity<ReceivableDebtor>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_ReceivableDebtor_DebtorId");

            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.ToTable("ReceivableDebtor");

            entity.HasOne(d => d.Receivable)
                .WithOne(p => p.Debtor)
                .HasForeignKey<ReceivableDebtor>(d => d.ReceivableId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(p => p.DebtorAddress);

        });

        builder.Entity<ReceivableDebtorAddress>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK_ReceivableDebtorAddress_DebtorAddressId");

            entity.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity.ToTable("ReceivableDebtorAddress");

            entity.HasOne(d => d.Debtor)
                .WithOne(p => p.DebtorAddress)
                .HasForeignKey<ReceivableDebtorAddress>(d => d.DebtorId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
