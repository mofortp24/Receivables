using FluentValidation;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;

namespace ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
public class AddReceivablesCommandValidator : AbstractValidator<AddReceivablesCommand>
{
    public AddReceivablesCommandValidator()
    {
        RuleFor(x => x.Receivables.ReceivableList)
            .NotEmpty().WithMessage("At least one receivable must be provided");

        RuleForEach(x => x.Receivables.ReceivableList)
            .SetValidator(new ReceivableDtoValidator());
    }
}
