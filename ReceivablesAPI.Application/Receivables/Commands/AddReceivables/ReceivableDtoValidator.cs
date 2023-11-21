using FluentValidation;
using ReceivablesAPI.Application.Common.Providers;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;

namespace ReceivablesAPI.Application.Receivables.Commands.AddReceivables;

public class ReceivableDtoValidator : AbstractValidator<ReceivableDto>
{
    public ReceivableDtoValidator()
    {
        RuleFor(x => x.Reference)
            .NotEmpty().WithMessage(ValidationMessageProvider.ReferenceIsRequired);

        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage(ValidationMessageProvider.CurrencyCodeIsRequired);

        RuleFor(x => x.IssueDate)
            .NotEmpty().WithMessage(ValidationMessageProvider.IssueDateIsRequired);

        RuleFor(x => x.OpeningValue)
            .GreaterThan(0).WithMessage(ValidationMessageProvider.OpeningValueMustBeGreaterThanZero);

        RuleFor(x => x.PaidValue)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessageProvider.PaidValueMustBeGreaterThanOrEqualToZero);

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage(ValidationMessageProvider.DueDateIsRequired);

        RuleFor(x => x.DebtorName)
            .NotEmpty().WithMessage(ValidationMessageProvider.DebtorNameIsRequired);

        RuleFor(x => x.DebtorReference)
            .NotEmpty().WithMessage(ValidationMessageProvider.DebtorReferenceIsRequired);

        RuleFor(x => x.DebtorCountryCode)
            .NotEmpty().WithMessage(ValidationMessageProvider.DebtorCountryCodeIsRequired);
    }
}
