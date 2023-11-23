using ReceivablesAPI.Application.Common.Exceptions;
using FluentAssertions;
using ReceivablesAPI.Application.IntegrationTests;
using ReceivablesAPI.Application.Receivables.Queries.GetReceivablesSummary;

namespace ReceivablesAPI.Application.IntegrationTests.Receivables.Queries;

using static Testing;

public class GetReceivablesSummaryTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new GetReceivablesOpenClosedSummaryQuery()
        {
            ReceivablesSummaryDate = null
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldAddReceivable()
    {
        var command = new GetReceivablesOpenClosedSummaryQuery()
        {
            ReceivablesSummaryDate = DateTime.Now
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().NotThrowAsync<ValidationException>();
    }
}
