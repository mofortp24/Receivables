using FluentValidation;
using ReceivablesAPI.Application.Common.Providers;

namespace ReceivablesAPI.Application.Receivables.Queries.GetReceivablesSummary;
public class GetReceivablesOpenClosedSummaryQueryValidator : AbstractValidator<GetReceivablesOpenClosedSummaryQuery>
{
    public GetReceivablesOpenClosedSummaryQueryValidator()
    {
        RuleFor(x => x.ReceivablesSummaryDate)
            .NotEmpty().WithMessage(ValidationMessageProvider.ReceivablesSummaryDateMustBeProvided);

        RuleFor(x => x.ReceivablesSummaryDate)
            .LessThan(x => DateTime.Now)
            .WithMessage(ValidationMessageProvider.ReceivablesSummaryDateMustNotBeInTheFuture);
    }
}
