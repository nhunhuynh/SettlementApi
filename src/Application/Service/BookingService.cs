using Application.Model;
using Application.Settlement.Command;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Application.Service
{
    public class BookingService: IBookingService
    {
        private readonly IBookingStoreProvider _storeProvider;

        public BookingService(IBookingStoreProvider storeProvider)
        {
            _storeProvider = storeProvider;
        }

        public async Task<BookingResult> ReserveBooking(BookSettlementCommand settlementCommand)
        {
            var booking = new Booking()
            {
                BookingTime = DateTime.ParseExact(settlementCommand.BookingTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
                Name = settlementCommand.Name
            };
            var bookingId = await _storeProvider.AddBooking(booking);
            var bookingResult = new BookingResult()
            {
                BookingId = bookingId,
                Outcome = bookingId.HasValue ? BookingStatus.Successful : BookingStatus.AlreadyReserved
            };
            return bookingResult;         
        }
    }
}
