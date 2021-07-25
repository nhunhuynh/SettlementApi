using Application.Service;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Settlement.Command
{
    public class BookSettlementCommandHandler : IRequestHandler<BookSettlementCommand, BookingResult>
    {
        private readonly IBookingService _bookingService;

        public BookSettlementCommandHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<BookingResult> Handle(BookSettlementCommand request, CancellationToken cancellationToken)
        {            
            return await _bookingService.ReserveBooking(request);
        }
    }
}
