using FluentValidation;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;

namespace ReceivablesAPI.Application.Receivables.Commands.AddReceivables;

public class ReceivableDtoValidator : AbstractValidator<ReceivableDto>
{
    public ReceivableDtoValidator()
    {
        RuleFor(x => x.Reference)
            .NotEmpty().WithMessage("Reference is required");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("Currency code is required");

        RuleFor(x => x.IssueDate)
            .NotEmpty().WithMessage("Issue date is required");

        RuleFor(x => x.OpeningValue)
            .GreaterThan(0).WithMessage("Opening value must be greater than zero");

        RuleFor(x => x.PaidValue)
            .GreaterThanOrEqualTo(0).WithMessage("Paid value must be greater than or equal to zero");

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage("Due date is required");

        RuleFor(x => x.DebtorName)
            .NotEmpty().WithMessage("Debtor name is required");

        RuleFor(x => x.DebtorReference)
            .NotEmpty().WithMessage("Debtor reference is required");

        RuleFor(x => x.DebtorCountryCode)
            .NotEmpty().WithMessage("Debtor country code is required");
    }
}
