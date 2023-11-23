using ReceivablesAPI.Application.Common.Exceptions;
using FluentAssertions;
using ReceivablesAPI.Application.IntegrationTests.TestFeed;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;

namespace ReceivablesAPI.Application.IntegrationTests.Receivables.Commands;


using static Testing;

public class AddReceivablesCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new AddReceivablesCommand()
        {
            Receivables = new ReceivablesDto(){
                ReceivableList = new List<ReceivableDto>(){
                    
                }
            }
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldAddReceivable()
    {
        var command = new AddReceivablesCommand()
        {
            Receivables = new ReceivablesDto(){
                ReceivableList = new List<ReceivableDto>(){
                    CorrectReceivableDtoFeed.GetTestInstance()
                }
            }
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().NotThrowAsync<ValidationException>();
    }
}
