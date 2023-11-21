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
                var entity = _mapper.Map<Receivable>(receivableDto);

                if (entity.Debtor.Id == 0 && !string.IsNullOrEmpty(entity.Debtor.DebtorReference))
                {
                    var debtorFromDb = _context.ReceivableDebtors.FirstOrDefault(d =>
                        d.DebtorReference == entity.Debtor.DebtorReference && d.DebtorName == entity.Debtor.DebtorName);

                    if (debtorFromDb is not null)
                        entity.Debtor = debtorFromDb;

                    var entityAddress = entity.DebtorAddress;

                    var debtorAddressFromDb = _context.ReceivableDebtorAddresses.FirstOrDefault(da =>
                                ((da.DebtorAddress1 == null && entityAddress.DebtorAddress1 == null) || da.DebtorAddress1 == entityAddress.DebtorAddress1)
                                && ((da.DebtorAddress2 == null && entityAddress.DebtorAddress2 == null) || da.DebtorAddress2 == entityAddress.DebtorAddress2)
                                && ((da.DebtorTown == null && entityAddress.DebtorTown == null) || da.DebtorTown == entityAddress.DebtorTown)
                                && ((da.DebtorState == null && entityAddress.DebtorState == null) || da.DebtorState == entityAddress.DebtorState)
                                && ((da.DebtorZip == null && entityAddress.DebtorZip == null) || da.DebtorZip == entityAddress.DebtorZip)
                                && da.DebtorCountryCode == entityAddress.DebtorCountryCode
                                && ((da.DebtorRegistrationNumber == null && entityAddress.DebtorRegistrationNumber == null) || da.DebtorRegistrationNumber == entityAddress.DebtorRegistrationNumber)
                            );

                    if (debtorAddressFromDb is not null)
                        entity.DebtorAddress = debtorAddressFromDb;

                //if (debtorFromDb is not null)
                //{
                //    var entityAddress = entity.DebtorAddress;

                //    //if (entityAddress is not null)
                //    //{
                //        var debtorAddressFromDb = debtorFromDb.DebtorAddresses.FirstOrDefault(da =>
                //            ((da.DebtorAddress1 is null && entityAddress.DebtorAddress1 is null) || da.DebtorAddress1 == entityAddress.DebtorAddress1)
                //            && ((da.DebtorAddress2 is null && entityAddress.DebtorAddress2 is null) || da.DebtorAddress2 == entityAddress.DebtorAddress2)
                //            && ((da.DebtorTown is null && entityAddress.DebtorTown is null) || da.DebtorTown == entityAddress.DebtorTown)
                //            && ((da.DebtorState is null && entityAddress.DebtorState is null) || da.DebtorState == entityAddress.DebtorState)
                //            && ((da.DebtorZip is null && entityAddress.DebtorZip is null) || da.DebtorZip == entityAddress.DebtorZip)
                //            && da.DebtorCountryCode == entityAddress.DebtorCountryCode
                //            && ((da.DebtorRegistrationNumber is null && entityAddress.DebtorRegistrationNumber is null) || da.DebtorRegistrationNumber == entityAddress.DebtorRegistrationNumber)
                //        );

                //        if (debtorAddressFromDb is null)
                //            debtorFromDb.DebtorAddresses.Add(entityAddress);

                //        entity.DebtorAddress = entityAddress;
                //    //}

                //    entity.Debtor = debtorFromDb;
                
                }

                batch.Receivables.Add(entity);
                
                entity.AddDomainEvent(new ReceivableCreatedEvent(entity));
            }

            _context.ReceivableBatches.Add(batch);

            await _context.SaveChangesAsync(cancellationToken);

            return batch.BatchReference;
        }
    }
