//using MediatR;
//using Microsoft.AspNetCore.Mvc;

//namespace ReceivablesAPI.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ReceivablesController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public ReceivablesController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddReceivables(ReceivablePayload payload)
//        {
//            // Store the receivables data in your data store
//            // You can use a database or an in-memory collection for simplicity

//            // Example: storing the data in an in-memory collection
//            var receivables = payload.Receivables;
//            // Store the receivables in your data store

//            return Ok();
//        }

//        [HttpGet("summary")]
//        public async Task<IActionResult> GetReceivablesSummary()
//        {
//            // Retrieve the stored receivables data from your data store

//            // Calculate the summary statistics
//            decimal openInvoicesValue = 0;
//            decimal closedInvoicesValue = 0;
//            // Calculate the summary statistics based on the stored data

//            var summary = new ReceivableSummary
//            {
//                OpenInvoicesValue = openInvoicesValue,
//                ClosedInvoicesValue = closedInvoicesValue
//            };

//            return Ok(new ReceivableSummaryResponse { Summary = summary });
//        }
//    }
//}
