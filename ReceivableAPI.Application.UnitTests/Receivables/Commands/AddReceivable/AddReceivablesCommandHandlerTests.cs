using AutoMapper;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using ReceivablesAPI.Application.Common.Interfaces;
using ReceivablesAPI.Application.Common.Mapping;
using ReceivablesAPI.Application.Common.Providers;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;
using ReceivablesAPI.Domain.Entities;
using ReceivablesAPI.Domain.Enums;
using ReceivablesAPI.Domain.Events;

namespace ReceivableAPI.Application.UnitTests.Receivables.Commands.AddReceivable;

[TestFixture]
public class AddReceivablesCommandHandlerTests
{
    private DbContext _dbContext;
    private IConfigurationProvider _configuration;
    private IApplicationDbContext _context;
    private IMapper _mapper;
    private IBatchReferenceProvider _batchReferenceProvider;

    [SetUp]
    public void Setup()
    {
        _dbContext = A.Fake<DbContext>();
        _context = A.Fake<IApplicationDbContext>();
        _configuration = new MapperConfiguration(config => 
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
        _batchReferenceProvider = new BatchReferenceProvider();
    
    }

    private DbSet<T> PrepareFakeDbSet<T>(List<T> collectionUnderFakeDbSet)  where T : class
    {
        var fakeDbSet = A.Fake<DbSet<T>>(options => options.Implements(typeof(IQueryable<T>)));
        A.CallTo(() => ((IQueryable<T>)fakeDbSet).Provider).Returns(collectionUnderFakeDbSet.AsQueryable().Provider);
        A.CallTo(() => ((IQueryable<T>)fakeDbSet).Expression).Returns(collectionUnderFakeDbSet.AsQueryable().Expression);
        A.CallTo(() => ((IQueryable<T>)fakeDbSet).ElementType).Returns(collectionUnderFakeDbSet.AsQueryable().ElementType);
        A.CallTo(() => ((IQueryable<T>)fakeDbSet).GetEnumerator()).Returns(collectionUnderFakeDbSet.GetEnumerator());

        A.CallTo(() => fakeDbSet.Add(A<T>._)).Invokes((T receivableBatch) =>
        {
            collectionUnderFakeDbSet.Add(receivableBatch);
        });

        return fakeDbSet;
    }

    [Test]
    public async Task Handler_Should_Add_New_Receivable_New_Debtor_And_New_Debtor_Address()
    {

        // Arrange
        // Receivable Batches
        var receivableBatches = new List<ReceivableBatch>();
        var fakeReceivableBatchSet = this.PrepareFakeDbSet<ReceivableBatch>(receivableBatches);
        A.CallTo(() => _context.ReceivableBatches).Returns(fakeReceivableBatchSet);
        
        // Receivables
        var receivables = new List<Receivable>()
        {
            new Receivable()
            {
                Id = 1,
                Batch = new ReceivableBatch()
                {
                    Id = 1,
                    BatchReference = "BATCH-1"
                },
                Reference = "REF-01",
                    
                DebtorId = 2,
                Debtor = new ReceivableDebtor()
                {
                    Id = 2,
                    DebtorReference = "DebtorReference_1", 
                    DebtorName = "DebtorName_1"
                },
                DebtorAddressId = 5,
                DebtorAddress = new ReceivableDebtorAddress()
                {
                    Id = 5,
                    DebtorAddress1 = "DebtorAddress1_1", 
                    DebtorAddress2 = "DebtorAddress1_2",
                    DebtorCountryCode = CountryCode.PL,
                }
                    
            }
        };

        var fakeReceivableSet = this.PrepareFakeDbSet<Receivable>(receivables);
        A.CallTo(() => _context.Receivables).Returns(fakeReceivableSet);
        
        // Receivable Debtors
        var receivableDebtors = new List<ReceivableDebtor>(){ 
            receivables.First().Debtor
        };
        
        var fakeReceivableDebtorSet = this.PrepareFakeDbSet<ReceivableDebtor>(receivableDebtors);
        A.CallTo(() => _context.ReceivableDebtors).Returns(fakeReceivableDebtorSet);

        // Receivable Debtors Addresses
        var receivableDebtorAddresses = new List<ReceivableDebtorAddress>(){ 
            receivables.First().DebtorAddress
        };

        var fakeReceivableDebtorAddressAddressSet = this.PrepareFakeDbSet<ReceivableDebtorAddress>(receivableDebtorAddresses);
        A.CallTo(() => _context.ReceivableDebtorAddresses).Returns(fakeReceivableDebtorAddressAddressSet);

        var request= new AddReceivablesCommand()
        {
            Receivables = new ReceivablesDto()
            {
                ReceivableList = new List<ReceivableDto>()
                {
                    new () { Reference = "NEW-REF-2", DebtorReference = "DebtorReference_NEW", DebtorName = "DebtorName_1", DebtorAddress1 = "DebtorAddress1_NEW", DebtorAddress2 = "DebtorAddress1_2", DebtorCountryCode = "PL" }
                }
            }
        };

        A.CallTo(() => _context.SaveChangesAsync(default))
            .Returns(Task.FromResult(0));

        
        var handler = new AddReceivablesCommandHandler(_context, _mapper, _batchReferenceProvider);

        var noOfReceivableBatchesBefore = _context.ReceivableBatches.Count();

        // Act
        var result = await handler.Handle(request, default).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);

        A.CallTo(() => _context.ReceivableBatches.Add(null!)).WithAnyArguments().MustHaveHappenedOnceExactly();

        Assert.That(() => _batchReferenceProvider.GenerateNextBatchReference<Receivable>(default).Length == result.Length);
        Assert.That(() => ++noOfReceivableBatchesBefore == _context.ReceivableBatches.Count());

        Assert.That(() => 1 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableCreatedEvent)))));

        Assert.That(() => 1 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableDebtorCreatedEvent)))));
        Assert.That(() => 1 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableDebtorAddressCreatedEvent)))));

        A.CallTo(() => _context.SaveChangesAsync(default)).WithAnyArguments().MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Handler_Should_Add_New_Receivable_Find_Existing_Debtor_And_Existing_Debtor_Address()
    {
        // Arrange
        // Receivable Batches
        var receivableBatches = new List<ReceivableBatch>();
        var fakeReceivableBatchSet = this.PrepareFakeDbSet<ReceivableBatch>(receivableBatches);
        A.CallTo(() => _context.ReceivableBatches).Returns(fakeReceivableBatchSet);
        
        // Receivables
        var receivables = new List<Receivable>()
        {
            new Receivable()
            {
                Id = 1,
                Batch = new ReceivableBatch()
                {
                    Id = 1,
                    BatchReference = "BATCH-1"
                },
                Reference = "REF-01",
                    
                DebtorId = 2,
                Debtor = new ReceivableDebtor()
                {
                    Id = 2,
                    DebtorReference = "DebtorReference_1", 
                    DebtorName = "DebtorName_1"
                },
                DebtorAddressId = 5,
                DebtorAddress = new ReceivableDebtorAddress()
                {
                    Id = 5,
                    DebtorAddress1 = "DebtorAddress1_1", 
                    DebtorAddress2 = "DebtorAddress1_2",
                    DebtorCountryCode = CountryCode.PL,
                }
                    
            }
        };

        var fakeReceivableSet = this.PrepareFakeDbSet<Receivable>(receivables);
        A.CallTo(() => _context.Receivables).Returns(fakeReceivableSet);
        
        // Receivable Debtors
        var receivableDebtors = new List<ReceivableDebtor>(){ 
            receivables.First().Debtor
        };
        
        var fakeReceivableDebtorSet = this.PrepareFakeDbSet<ReceivableDebtor>(receivableDebtors);
        A.CallTo(() => _context.ReceivableDebtors).Returns(fakeReceivableDebtorSet);

        // Receivable Debtors Addresses
        var receivableDebtorAddresses = new List<ReceivableDebtorAddress>(){ 
            receivables.First().DebtorAddress
        };

        var fakeReceivableDebtorAddressAddressSet = this.PrepareFakeDbSet<ReceivableDebtorAddress>(receivableDebtorAddresses);
        A.CallTo(() => _context.ReceivableDebtorAddresses).Returns(fakeReceivableDebtorAddressAddressSet);

        var request= new AddReceivablesCommand()
        {
            Receivables = new ReceivablesDto()
            {
                ReceivableList = new List<ReceivableDto>()
                {
                    new () { Reference = "NEW-REF-1", DebtorReference = "DebtorReference_1", DebtorName = "DebtorName_1", DebtorAddress1 = "DebtorAddress1_1", DebtorAddress2 = "DebtorAddress1_2", DebtorCountryCode = "PL" }
                }
            }
        };

        A.CallTo(() => _context.SaveChangesAsync(default))
            .Returns(Task.FromResult(0));

        
        var handler = new AddReceivablesCommandHandler(_context, _mapper, _batchReferenceProvider);

        var noOfReceivableBatchesBefore = _context.ReceivableBatches.Count();

        // Act
        var result = await handler.Handle(request, default).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);

        A.CallTo(() => _context.ReceivableBatches.Add(null!)).WithAnyArguments().MustHaveHappenedOnceExactly();

        Assert.That(() => _batchReferenceProvider.GenerateNextBatchReference<Receivable>(default).Length == result.Length);
        Assert.That(() => ++noOfReceivableBatchesBefore == _context.ReceivableBatches.Count());

        Assert.That(() => 1 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableCreatedEvent)))));

        Assert.That(() => 0 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableDebtorCreatedEvent)))));
        Assert.That(() => 0 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableDebtorAddressCreatedEvent)))));

        A.CallTo(() => _context.SaveChangesAsync(default)).WithAnyArguments().MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Handler_Should_Add_New_Receivable_Find_Existing_Debtor_And_Add_New_Debtor_Address()
    {
        // Arrange
        // Receivable Batches
        var receivableBatches = new List<ReceivableBatch>();
        var fakeReceivableBatchSet = this.PrepareFakeDbSet<ReceivableBatch>(receivableBatches);
        A.CallTo(() => _context.ReceivableBatches).Returns(fakeReceivableBatchSet);
        
        // Receivables
        var receivables = new List<Receivable>()
        {
            new Receivable()
            {
                Id = 1,
                Batch = new ReceivableBatch()
                {
                    Id = 1,
                    BatchReference = "BATCH-1"
                },
                Reference = "REF-01",
                    
                DebtorId = 2,
                Debtor = new ReceivableDebtor()
                {
                    Id = 2,
                    DebtorReference = "DebtorReference_1", 
                    DebtorName = "DebtorName_1"
                },
                DebtorAddressId = 5,
                DebtorAddress = new ReceivableDebtorAddress()
                {
                    Id = 5,
                    DebtorAddress1 = "DebtorAddress1_1", 
                    DebtorAddress2 = "DebtorAddress1_2",
                    DebtorCountryCode = CountryCode.PL,
                }
                    
            }
        };

        var fakeReceivableSet = this.PrepareFakeDbSet<Receivable>(receivables);
        A.CallTo(() => _context.Receivables).Returns(fakeReceivableSet);
        
        // Receivable Debtors
        var receivableDebtors = new List<ReceivableDebtor>(){ 
            receivables.First().Debtor
        };
        
        var fakeReceivableDebtorSet = this.PrepareFakeDbSet<ReceivableDebtor>(receivableDebtors);
        A.CallTo(() => _context.ReceivableDebtors).Returns(fakeReceivableDebtorSet);

        // Receivable Debtors Addresses
        var receivableDebtorAddresses = new List<ReceivableDebtorAddress>(){ 
            receivables.First().DebtorAddress
        };

        var fakeReceivableDebtorAddressAddressSet = this.PrepareFakeDbSet<ReceivableDebtorAddress>(receivableDebtorAddresses);
        A.CallTo(() => _context.ReceivableDebtorAddresses).Returns(fakeReceivableDebtorAddressAddressSet);

        var request= new AddReceivablesCommand()
        {
            Receivables = new ReceivablesDto()
            {
                ReceivableList = new List<ReceivableDto>()
                {
                    new () { Reference = "NEW-REF-1", DebtorReference = "DebtorReference_1", DebtorName = "DebtorName_1", DebtorAddress1 = "DebtorAddress1_1", DebtorAddress2 = "DebtorAddress1_NEW", DebtorCountryCode = "PL" }
                }
            }
        };

        A.CallTo(() => _context.SaveChangesAsync(default))
            .Returns(Task.FromResult(0));

        
        var handler = new AddReceivablesCommandHandler(_context, _mapper, _batchReferenceProvider);

        var noOfReceivableBatchesBefore = _context.ReceivableBatches.Count();

        // Act
        var result = await handler.Handle(request, default).ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(result);

        A.CallTo(() => _context.ReceivableBatches.Add(null!)).WithAnyArguments().MustHaveHappenedOnceExactly();

        Assert.That(() => _batchReferenceProvider.GenerateNextBatchReference<Receivable>(default).Length == result.Length);
        Assert.That(() => ++noOfReceivableBatchesBefore == _context.ReceivableBatches.Count());

        Assert.That(() => 1 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableCreatedEvent)))));

        Assert.That(() => 0 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableDebtorCreatedEvent)))));
        Assert.That(() => 1 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableDebtorAddressCreatedEvent)))));

        A.CallTo(() => _context.SaveChangesAsync(default)).WithAnyArguments().MustHaveHappenedOnceExactly();
    }
}