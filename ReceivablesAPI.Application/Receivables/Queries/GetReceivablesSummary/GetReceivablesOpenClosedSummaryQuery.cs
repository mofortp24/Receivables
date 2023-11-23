using AutoMapper;
using ReceivablesAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ReceivablesAPI.Application.Receivables.Queries.GetReceivablesSummary;

public record GetReceivablesOpenClosedSummaryQuery : IRequest<ReceivablesOpenClosedSummary>
{
    public DateTime? ReceivablesSummaryDate { get; init; }
}

public class GetReceivablesSummaryQueryHandler : IRequestHandler<GetReceivablesOpenClosedSummaryQuery, ReceivablesOpenClosedSummary>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReceivablesSummaryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReceivablesOpenClosedSummary> Handle(GetReceivablesOpenClosedSummaryQuery request, CancellationToken cancellationToken)
    {
        var receivableStatsTill = request.ReceivablesSummaryDate ?? DateTime.Now;

        var temp = _context.Receivables.Select(r => r);

        var openCloseCounters = await _context.Receivables
            .Where(r => DateTime.Compare(r.Created, receivableStatsTill) < 0)
            .GroupBy(r => new { IsOpen = (r.ClosedDate.HasValue || (r.Cancelled.HasValue && r.Cancelled.Value) ? 0 : 1), IsCosed = (r.ClosedDate.HasValue || (r.Cancelled.HasValue && r.Cancelled.Value) ? 1 : 0) })
            .Select(g => new
            {
                g.Key.IsOpen,
                g.Key.IsCosed,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return new ReceivablesOpenClosedSummary
        {
            GenerationDate = DateTime.Now,
            ReceivablesSummary = new ReceivablesOpenClosedSummaryDto(){
                OpenedReceivables = openCloseCounters.FirstOrDefault(c => c.IsOpen == 1)?.Count ?? 0,
                ClosedReceivables = openCloseCounters.FirstOrDefault(c => c.IsCosed == 1)?.Count ?? 0
            }
        };
    }
}