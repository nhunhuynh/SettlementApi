using Application.Model;
using System;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface IBookingStoreProvider
    {
        Task<Guid?> AddBooking(Booking booking);        
    }
}
