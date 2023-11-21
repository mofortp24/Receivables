namespace ReceivablesAPI.Application.Receivables.Queries.GetReceivablesSummary;
public class ReceivablesOpenClosedSummary
{
    public DateTime GenerationDate { get; init; }

    public ReceivablesOpenClosedSummaryDto ReceivablesSummary { get; init; } = new ();
}
