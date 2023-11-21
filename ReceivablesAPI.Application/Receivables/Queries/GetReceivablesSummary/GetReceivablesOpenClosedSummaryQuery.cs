using AutoMapper;
using AutoMapper.QueryableExtensions;
using ReceivablesAPI.Application.Common.Interfaces;
//using ReceivablesAPI.Application.Common.Security;
using ReceivablesAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ReceivablesAPI.Application.Receivables.Queries.GetReceivablesSummary;

//[Authorize]
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
        var receivableStatsTill = (request.ReceivablesSummaryDate?.Date ?? DateTime.Now);
        var openCloseCounters = await _context.Receivables
            .Where(r => DateTime.Compare(r.Created, receivableStatsTill) < 0)
            .GroupBy(r => new { IsOpen = (r.ClosedDate.HasValue ? 0 : 1), IsCosed = (r.ClosedDate.HasValue ? 1 : 0) })
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