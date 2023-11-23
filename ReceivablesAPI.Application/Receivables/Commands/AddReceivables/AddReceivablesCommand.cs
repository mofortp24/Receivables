using AutoMapper;
using MediatR;
using ReceivablesAPI.Application.Common.Interfaces;
using ReceivablesAPI.Application.Common.Providers;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;
using ReceivablesAPI.Domain.Entities;
using ReceivablesAPI.Domain.Events;

namespace ReceivablesAPI.Application.Receivables.Commands.AddReceivables;

    public record AddReceivablesCommand : IRequest<string>
    {
        public ReceivablesDto Receivables { get; init; } = new ReceivablesDto();
    }

    public class AddReceivablesCommandHandler : IRequestHandler<AddReceivablesCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBatchReferenceProvider _batchReferenceProvider;

        public AddReceivablesCommandHandler(IApplicationDbContext context, IMapper mapper, IBatchReferenceProvider batchReferenceProvider)
        {
            _context = context;
            _mapper = mapper;
            _batchReferenceProvider = batchReferenceProvider;
        }


        public async Task<string> Handle(AddReceivablesCommand request, CancellationToken cancellationToken)
        {
            var batch = new ReceivableBatch()
            {
                BatchReference = _batchReferenceProvider.GenerateNextBatchReference<Receivable>(cancellationToken)
            };

            foreach (var receivableDto in request.Receivables.ReceivableList)
            {
                var receivableEntity = _mapper.Map<Receivable>(receivableDto);

                if (receivableEntity.Debtor.Id == 0 && !string.IsNullOrEmpty(receivableEntity.Debtor.DebtorReference))
                {
                    var debtorFromDb = _context.ReceivableDebtors.FirstOrDefault(d =>
                        d.DebtorReference == receivableEntity.Debtor.DebtorReference && d.DebtorName == receivableEntity.Debtor.DebtorName);

                    if (debtorFromDb is not null)
                        receivableEntity.Debtor = debtorFromDb;

                    var entityAddress = receivableEntity.DebtorAddress;

                    if (receivableEntity.Debtor.Id > 0)
                    {
                        var debtorAddressFromDb = _context.Receivables
                            .Join(_context.ReceivableDebtorAddresses,
                                receivable => receivable.DebtorAddressId,
                                debtorAddress => debtorAddress.Id,
                                (receivable, debtorAddress) => new { Receivable = receivable, DebtorAddress = debtorAddress })
                            .Where(joinResult => joinResult.Receivable.DebtorId == receivableEntity.Debtor.Id)
                            .Select(joinResult => joinResult.DebtorAddress)
                            .FirstOrDefault(da =>
                                ((da.DebtorAddress1 == null && entityAddress.DebtorAddress1 == null) || da.DebtorAddress1 == entityAddress.DebtorAddress1)
                                && ((da.DebtorAddress2 == null && entityAddress.DebtorAddress2 == null) || da.DebtorAddress2 == entityAddress.DebtorAddress2)
                                && ((da.DebtorTown == null && entityAddress.DebtorTown == null) || da.DebtorTown == entityAddress.DebtorTown)
                                && ((da.DebtorState == null && entityAddress.DebtorState == null) || da.DebtorState == entityAddress.DebtorState)
                                && ((da.DebtorZip == null && entityAddress.DebtorZip == null) || da.DebtorZip == entityAddress.DebtorZip)
                                && da.DebtorCountryCode == entityAddress.DebtorCountryCode
                                && ((da.DebtorRegistrationNumber == null && entityAddress.DebtorRegistrationNumber == null) || da.DebtorRegistrationNumber == entityAddress.DebtorRegistrationNumber)
                            );

                        if (debtorAddressFromDb is not null)
                            receivableEntity.DebtorAddress = debtorAddressFromDb;
                    }
                }

                batch.Receivables.Add(receivableEntity);
                
                receivableEntity.AddDomainEvent(new ReceivableCreatedEvent(receivableEntity));
                if(receivableEntity.Debtor.Id == 0)
                    receivableEntity.AddDomainEvent(new ReceivableDebtorCreatedEvent(receivableEntity.Debtor));
                if(receivableEntity.DebtorAddress.Id == 0)
                    receivableEntity.AddDomainEvent(new ReceivableDebtorAddressCreatedEvent(receivableEntity.DebtorAddress));
            }

            _context.ReceivableBatches.Add(batch);

            await _context.SaveChangesAsync(cancellationToken);

            return batch.BatchReference;
        }
    }
