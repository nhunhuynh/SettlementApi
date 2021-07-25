using Application.Settlement.Command;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface IBookingService
    {
        Task<BookingResult> ReserveBooking(BookSettlementCommand settlementCommand);
    }
}
