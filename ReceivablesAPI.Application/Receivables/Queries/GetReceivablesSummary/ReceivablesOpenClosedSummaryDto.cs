namespace ReceivablesAPI.Application.Receivables.Queries.GetReceivablesSummary;

public class ReceivablesOpenClosedSummaryDto
{
    public int OpenedReceivables { get; init; }

    public int ClosedReceivables { get; init; }
}