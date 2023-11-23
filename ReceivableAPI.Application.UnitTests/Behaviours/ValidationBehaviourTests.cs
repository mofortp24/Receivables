using FakeItEasy;
using FluentValidation;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using ReceivablesAPI.Application.Common.Behaviours;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
using ValidationException = ReceivablesAPI.Application.Common.Exceptions.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;
using ReceivablesAPI.Application.Common.Providers;

namespace ReceivablesAPI.Application.UnitTests.Behaviours;
[TestFixture]
public class ValidationBehaviourTests
{
    [Test]
    public async Task Handle_ValidRequest_NoExceptionThrown()
    {
        // Arrange
        var validators = new List<IValidator<AddReceivablesCommand>>();
        var behavior = new ValidationBehaviour<AddReceivablesCommand, string>(validators);
        var request = new AddReceivablesCommand();
        var nextHandler = A.Fake<RequestHandlerDelegate<string>>();

        // Act
        Func<Task> act = async () => await behavior.Handle(request, nextHandler, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Handle_InvalidRequest_ValidationExceptionThrown()
    {
        // Arrange
        var validator = A.Fake<IValidator<AddReceivablesCommand>>();
        A.CallTo(() => validator.ValidateAsync(A<ValidationContext<AddReceivablesCommand>>._, A<CancellationToken>._))
            .Returns(new ValidationResult(new List<ValidationFailure> { new ("CurrencyCode", ValidationMessageProvider.CurrencyCodeIsRequired) }));

        var validators = new List<IValidator<AddReceivablesCommand>> { validator };
        var behavior = new ValidationBehaviour<AddReceivablesCommand, string>(validators);
        var request = new AddReceivablesCommand();
        var nextHandler = A.Fake<RequestHandlerDelegate<string>>();

        // Act
        Func<Task> act = async () => await behavior.Handle(request, nextHandler, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task Handle_AllValidatorsValid_NoExceptionThrown()
    {
        // Arrange
        var validator1 = A.Fake<IValidator<AddReceivablesCommand>>();
        A.CallTo(() => validator1.ValidateAsync(A<ValidationContext<AddReceivablesCommand>>._, A<CancellationToken>._))
            .Returns(new ValidationResult());

        var validator2 = A.Fake<IValidator<AddReceivablesCommand>>();
        A.CallTo(() => validator2.ValidateAsync(A<ValidationContext<AddReceivablesCommand>>._, A<CancellationToken>._))
            .Returns(new ValidationResult());

        var validators = new List<IValidator<AddReceivablesCommand>> { validator1, validator2 };
        var behavior = new ValidationBehaviour<AddReceivablesCommand, string>(validators);
        var request = new AddReceivablesCommand();
        var nextHandler = A.Fake<RequestHandlerDelegate<string>>();

        // Act
        Func<Task> act = async () => await behavior.Handle(request, nextHandler, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync<ValidationException>();
    }
}

