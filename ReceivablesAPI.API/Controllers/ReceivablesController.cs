using System.Runtime.Serialization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReceivablesAPI.Application.Receivables.Commands.AddReceivables;
using ReceivablesAPI.Application.Receivables.Queries.GetReceivablesSummary;
using ReceivablesAPI.WebUI.Controllers;

namespace ReceivablesAPI.API.Controllers
{
    //[Authorize()]
    public class ReceivablesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ReceivablesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost("[action]")]
        public async Task<string> Add(AddReceivablesCommand command)
        {
            return await Mediator.Send(command);

        }

        [HttpGet("/stats/[action]")]
        public async Task<ActionResult<ReceivablesOpenClosedSummary>> GetReceivablesOpenClosedSummary([FromQuery] GetReceivablesOpenClosedSummaryQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}