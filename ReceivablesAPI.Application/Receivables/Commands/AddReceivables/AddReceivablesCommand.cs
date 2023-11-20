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
                batch.Receivables.Add(entity);
                
                entity.AddDomainEvent(new ReceivableCreatedEvent(entity));
            }

            _context.ReceivableBatches.Add(batch);

            await _context.SaveChangesAsync(cancellationToken);

            return batch.BatchReference;
        }
    }
