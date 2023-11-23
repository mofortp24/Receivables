using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables.Dto;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
using ReceivablesAPI.Application.Receivables.Queries.GetReceivablesSummary;
using ReceivablesAPI.Application.UnitTests.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ReceivablesAPI.Application.UnitTests.Receivables.Queries.GetReceivablesSummary;

[TestFixture]
public class GetReceivablesOpenClosedSummaryQueryValidatorTests
{
    private GetReceivablesOpenClosedSummaryQueryValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GetReceivablesOpenClosedSummaryQueryValidator();
    }

    [Test]
    public void ReceivablesSummaryDate_Should_Not_Be_Empty_Or_In_The_Future()
    {
        // Empty ReceivablesSummaryDate should fail
        var query = new GetReceivablesOpenClosedSummaryQuery()
        {
            ReceivablesSummaryDate = null
        };
        _validator.ShouldHaveValidationErrorFor(query, x => x.ReceivablesSummaryDate, query.ReceivablesSummaryDate);
        
        // Future date for ReceivablesSummaryDate should fail
        query = new GetReceivablesOpenClosedSummaryQuery()
        {
            ReceivablesSummaryDate = DateTime.Now.AddDays(1)
        };
        _validator.ShouldHaveValidationErrorFor(query,x => x.ReceivablesSummaryDate, query.ReceivablesSummaryDate);

        // Non-empty ReceivablesSummaryDate should pass
        query = new GetReceivablesOpenClosedSummaryQuery()
        {
            ReceivablesSummaryDate = DateTime.Now
        };
        _validator.ShouldNotHaveValidationErrorFor(query,x => x.ReceivablesSummaryDate, query.ReceivablesSummaryDate);
    }
}
