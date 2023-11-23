using FluentValidation;
using ReceivablesAPI.Application.Common.Providers;

namespace ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
public class AddReceivablesCommandValidator : AbstractValidator<AddReceivablesCommand>
{
    public AddReceivablesCommandValidator()
    {
        RuleFor(x => x.Receivables.ReceivableList)
            .NotEmpty().WithMessage(ValidationMessageProvider.AtLeastOneReceivableMustBeProvided);

        RuleForEach(x => x.Receivables.ReceivableList)
            .SetValidator(new ReceivableDtoValidator());
    }
}
