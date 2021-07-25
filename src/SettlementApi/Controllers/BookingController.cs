using Application.Settlement.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SettlementApi.Payloads;
using System.Threading.Tasks;

namespace SettlementApi.Controllers
{
    [Route("[controller]")]
    public class BookingController: ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("")]
        public async Task<ActionResult<BookingResult>> Book([FromBody] BookSettlementCommand command)
        {
            var bookingResult = await _mediator.Send(command);
            if (!bookingResult.BookingId.HasValue)
                return Conflict("The requested booking time has already been reserved.");
            return Ok(new BookingResponse() { BookingId = bookingResult.BookingId.Value});            
        }
    }
}
