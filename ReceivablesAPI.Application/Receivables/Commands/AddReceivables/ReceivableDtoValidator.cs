using System.Globalization;
using FluentValidation;
using ReceivablesAPI.Application.Common.Providers;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;
using ReceivablesAPI.Domain.Enums;

namespace ReceivablesAPI.Application.Receivables.Commands.AddReceivables;

public class ReceivableDtoValidator : AbstractValidator<ReceivableDto>
{
    public ReceivableDtoValidator()
    {

        RuleFor(x => x.Reference)
            .NotEmpty().WithMessage(ValidationMessageProvider.ReferenceIsRequired)
            .MaximumLength(50).WithMessage(ValidationMessageProvider.ReferenceMaxLengthExceeded);

        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage(ValidationMessageProvider.CurrencyCodeIsRequired)
            .Length(3).WithMessage(ValidationMessageProvider.CurrencyCodeLengthIncorrect)
            .IsEnumName(typeof(CurrencyCode)).WithMessage(ValidationMessageProvider.CurrencyCodeFormatUnsupported);

        RuleFor(x => x.IssueDate)
            .NotEmpty().WithMessage(ValidationMessageProvider.IssueDateIsRequired)
            .Must(c => DateTime.TryParseExact(c, ValidationFormatProviders.DateTimeAcceptableFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            .WithMessage(ValidationMessageProvider.IssueDateIncorrectFormat);

        RuleFor(x => x.OpeningValue)
            .GreaterThan(0).WithMessage(ValidationMessageProvider.OpeningValueMustBeGreaterThanZero);

        RuleFor(x => x.PaidValue)
            .GreaterThanOrEqualTo(0).WithMessage(ValidationMessageProvider.PaidValueMustBeGreaterThanOrEqualToZero);

        RuleFor(x => x.DueDate)
            .NotEmpty().WithMessage(ValidationMessageProvider.DueDateIsRequired)
            .Must(c => DateTime.TryParseExact(c, ValidationFormatProviders.DateTimeAcceptableFormat , CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            .WithMessage(ValidationMessageProvider.DueDateIncorrectFormat);

        RuleFor(x => x.ClosedDate)
            .Must(c => (string.IsNullOrEmpty(c) || DateTime.TryParseExact(c, ValidationFormatProviders.DateTimeAcceptableFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)))
            .WithMessage(ValidationMessageProvider.ClosedDateIncorrectFormat);

        RuleFor(x => x.DebtorName)
            .NotEmpty().WithMessage(ValidationMessageProvider.DebtorNameIsRequired)
            .MaximumLength(100).WithMessage(ValidationMessageProvider.DebtorNameMaxLengthExceeded);

        RuleFor(x => x.DebtorReference)
            .NotEmpty().WithMessage(ValidationMessageProvider.DebtorReferenceIsRequired)
            .MaximumLength(50).WithMessage(ValidationMessageProvider.DebtorReferenceMaxLengthExceeded);


        RuleFor(x => x.DebtorAddress1)
            .MaximumLength(250).WithMessage(ValidationMessageProvider.DebtorAddress1MaxLengthExceeded);

        RuleFor(x => x.DebtorAddress2)
            .MaximumLength(250).WithMessage(ValidationMessageProvider.DebtorAddress2MaxLengthExceeded);

        RuleFor(x => x.DebtorTown)
            .MaximumLength(50).WithMessage(ValidationMessageProvider.DebtorTownMaxLengthExceeded);

        RuleFor(x => x.DebtorState)
            .MaximumLength(50).WithMessage(ValidationMessageProvider.DebtorStateMaxLengthExceeded);

        RuleFor(x => x.DebtorZip)
            .MaximumLength(10).WithMessage(ValidationMessageProvider.DebtorZipMaxLengthExceeded);

        RuleFor(x => x.DebtorCountryCode)
            .NotEmpty().WithMessage(ValidationMessageProvider.DebtorCountryCodeIsRequired)
            .Length(2).WithMessage(ValidationMessageProvider.DebtorCountryCodeLengthIncorrect)
            .IsEnumName(typeof(CountryCode)).WithMessage(ValidationMessageProvider.DebtorCountryCodeFormatUnsupported);

        RuleFor(x => x.DebtorRegistrationNumber)
            .MaximumLength(15).WithMessage(ValidationMessageProvider.DebtorRegistrationNumberMaxLengthExceeded);
    }
}

//