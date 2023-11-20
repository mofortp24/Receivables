using System.Runtime.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
using ReceivablesAPI.WebUI.Controllers;

namespace ReceivablesAPI.API.Controllers
{
    public class ReceivablesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ReceivablesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost]
        public async Task<string> Add(AddReceivablesCommand command)
        {
            return await Mediator.Send(command);

        }

        //[HttpGet("summary")]
        //public async Task<IActionResult> GetReceivablesSummary()
        //{
        //    // Retrieve the stored receivables data from your data store

        //    // Calculate the summary statistics
        //    decimal openInvoicesValue = 0;
        //    decimal closedInvoicesValue = 0;
        //    // Calculate the summary statistics based on the stored data

        //    var summary = new ReceivableSummary
        //    {
        //        OpenInvoicesValue = openInvoicesValue,
        //        ClosedInvoicesValue = closedInvoicesValue
        //    };

        //    return Ok(new ReceivableSummaryResponse { Summary = summary });
        //}
    }
}