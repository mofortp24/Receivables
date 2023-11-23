using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using ReceivablesAPI.Application.Common.Behaviours;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;

[TestFixture]
public class UnhandledExceptionBehaviourTests
{
    [Test]
    public async Task Handle_NoExceptionThrown()
    {
        // Arrange
        var logger = A.Fake<ILogger<AddReceivablesCommand>>();
        var behavior = new UnhandledExceptionBehaviour<AddReceivablesCommand, string>(logger);
        var request = new AddReceivablesCommand();
        var nextHandler = A.Fake<RequestHandlerDelegate<string>>();

        // Act
        Func<Task<string>> act = async () => await behavior.Handle(request, nextHandler, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }

    [Test]
    public async Task Handle_ExceptionThrown_ExceptionLoggedAndRethrown()
    {
        // Arrange
        var logger = A.Fake<Logger<AddReceivablesCommand>>();
        var behavior = new UnhandledExceptionBehaviour<AddReceivablesCommand, string>(logger);
        var request = new AddReceivablesCommand();
        var nextHandler = A.Fake<RequestHandlerDelegate<string>>();
        A.CallTo(() => nextHandler.Invoke()).Throws(new InvalidOperationException());

        // Act
        Func<Task<string>> act = async () => await behavior.Handle(request, nextHandler, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}