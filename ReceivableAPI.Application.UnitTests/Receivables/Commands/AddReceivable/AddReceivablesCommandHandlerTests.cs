using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
        //_mapper = A.Fake<IMapper>();
        _configuration = new MapperConfiguration(config => 
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
        _batchReferenceProvider = new BatchReferenceProvider();
    
    }

    [Test]
    public async Task Handler_Should_Add_New_Receivable_New_Debtor_And_New_Debtor_Address()
    {

        // Arrange
        // Receivable Batches
        var receivableBatches = new List<ReceivableBatch>();

        var fakeReceivableBatchSet = A.Fake<DbSet<ReceivableBatch>>(options => options.Implements(typeof(IQueryable<ReceivableBatch>)));
        A.CallTo(() => ((IQueryable<ReceivableBatch>)fakeReceivableBatchSet).Provider).Returns(receivableBatches.AsQueryable().Provider);
        A.CallTo(() => ((IQueryable<ReceivableBatch>)fakeReceivableBatchSet).Expression).Returns(receivableBatches.AsQueryable().Expression);
        A.CallTo(() => ((IQueryable<ReceivableBatch>)fakeReceivableBatchSet).ElementType).Returns(receivableBatches.AsQueryable().ElementType);
        A.CallTo(() => ((IQueryable<ReceivableBatch>)fakeReceivableBatchSet).GetEnumerator()).Returns(receivableBatches.GetEnumerator());

        A.CallTo(() => _context.ReceivableBatches).Returns(fakeReceivableBatchSet);

        A.CallTo(() => fakeReceivableBatchSet.Add(A<ReceivableBatch>._)).Invokes((ReceivableBatch receivableBatch) =>
        {
            receivableBatches.Add(receivableBatch);
        });

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

        var fakeReceivableSet = A.Fake<DbSet<Receivable>>(options => options.Implements(typeof(IQueryable<Receivable>)));
        A.CallTo(() => ((IQueryable<Receivable>)fakeReceivableSet).Provider).Returns(receivables.AsQueryable().Provider);
        A.CallTo(() => ((IQueryable<Receivable>)fakeReceivableSet).Expression).Returns(receivables.AsQueryable().Expression);
        A.CallTo(() => ((IQueryable<Receivable>)fakeReceivableSet).ElementType).Returns(receivables.AsQueryable().ElementType);
        A.CallTo(() => ((IQueryable<Receivable>)fakeReceivableSet).GetEnumerator()).Returns(receivables.GetEnumerator());

        A.CallTo(() => _context.Receivables).Returns(fakeReceivableSet);

        A.CallTo(() => fakeReceivableSet.Add(A<Receivable>._)).Invokes((Receivable receivable) =>
        {
            receivables.Add(receivable);
        });

        // Receivable Debtors
        var receivableDebtors = new List<ReceivableDebtor>(){ 
            new ReceivableDebtor()
            {
                Id = 2,
                DebtorReference = "DebtorReference_1",
                DebtorName = "DebtorName_1"
            }
        };
        
        var fakeReceivableDebtorSet = A.Fake<DbSet<ReceivableDebtor>>(options => options.Implements(typeof(IQueryable<ReceivableDebtor>)));;

        A.CallTo(() => ((IQueryable<ReceivableDebtor>)fakeReceivableDebtorSet).Provider).Returns(receivableDebtors.AsQueryable().Provider);
        A.CallTo(() => ((IQueryable<ReceivableDebtor>)fakeReceivableDebtorSet).Expression).Returns(receivableDebtors.AsQueryable().Expression);
        A.CallTo(() => ((IQueryable<ReceivableDebtor>)fakeReceivableDebtorSet).ElementType).Returns(receivableDebtors.AsQueryable().ElementType);
        A.CallTo(() => ((IQueryable<ReceivableDebtor>)fakeReceivableDebtorSet).GetEnumerator()).Returns(receivableDebtors.GetEnumerator());

        A.CallTo(() => _context.ReceivableDebtors).Returns(fakeReceivableDebtorSet);

        A.CallTo(() => fakeReceivableDebtorSet.Add(A<ReceivableDebtor>._)).Invokes((ReceivableDebtor receivableDebtor) =>
        {
            receivableDebtors.Add(receivableDebtor);
        });

        // Receivable Debtors Addresses
        var receivableDebtorAddresses = new List<ReceivableDebtorAddress>(){ 
            new ReceivableDebtorAddress()
            {
                Id = 5,
                DebtorAddress1 = "DebtorAddress1_1", 
                DebtorAddress2 = "DebtorAddress1_2",
                DebtorCountryCode = CountryCode.PL,
            }
        };

        var fakeReceivableDebtorAddressAddressSet = A.Fake<DbSet<ReceivableDebtorAddress>>(options => options.Implements(typeof(IQueryable<ReceivableDebtorAddress>)));;

        A.CallTo(() => ((IQueryable<ReceivableDebtorAddress>)fakeReceivableDebtorAddressAddressSet).Provider).Returns(receivableDebtorAddresses.AsQueryable().Provider);
        A.CallTo(() => ((IQueryable<ReceivableDebtorAddress>)fakeReceivableDebtorAddressAddressSet).Expression).Returns(receivableDebtorAddresses.AsQueryable().Expression);
        A.CallTo(() => ((IQueryable<ReceivableDebtorAddress>)fakeReceivableDebtorAddressAddressSet).ElementType).Returns(receivableDebtorAddresses.AsQueryable().ElementType);
        A.CallTo(() => ((IQueryable<ReceivableDebtorAddress>)fakeReceivableDebtorAddressAddressSet).GetEnumerator()).Returns(receivableDebtorAddresses.GetEnumerator());

        A.CallTo(() => _context.ReceivableDebtorAddresses).Returns(fakeReceivableDebtorAddressAddressSet);

        A.CallTo(() => fakeReceivableDebtorAddressAddressSet.Add(A<ReceivableDebtorAddress>._)).Invokes((ReceivableDebtorAddress receivableDebtorAddress) =>
        {
            receivableDebtorAddresses.Add(receivableDebtorAddress);
        });

        var request= new AddReceivablesCommand()
        {
            Receivables = new ReceivablesDto()
            {
                ReceivableList = new List<ReceivableDto>()
                {
                    new () { Reference = "REF-2", DebtorReference = "DebtorReference_1", DebtorName = "DebtorName_1", DebtorAddress1 = "DebtorAddress1_1", DebtorAddress2 = "DebtorAddress1_2", DebtorCountryCode = "PL" }
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

        A.CallTo(() => _context.ReceivableBatches.Add(null)).WithAnyArguments().MustHaveHappenedOnceExactly();

        Assert.That(() => _batchReferenceProvider.GenerateNextBatchReference<Receivable>(default).Length == result.Length);
        Assert.That(() => ++noOfReceivableBatchesBefore == _context.ReceivableBatches.Count());

        Assert.That(() => 1 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableCreatedEvent)))));

        Assert.That(() => 0 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableDebtorCreatedEvent)))));
        Assert.That(() => 0 ==_context.ReceivableBatches.Count(b => b.Receivables.Any(r => r.DomainEvents.Any(de => de.GetType() == typeof(ReceivableDebtorAddressCreatedEvent)))));

        A.CallTo(() => _context.SaveChangesAsync(default)).WithAnyArguments().MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Handler_Should_Add_New_Receivable_Find_Existing_Debtor_And_Existing_Debtor_Address() { }

    [Test]
    public void Handler_Should_Add_New_Receivable_Find_Existing_Debtor_And_Add_New_Debtor_Address() { }
}