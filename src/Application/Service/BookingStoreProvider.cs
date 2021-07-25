using Application.Model;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Service
{
    public class BookingStoreProvider: IBookingStoreProvider
    {
        private const int MaxBookingOverlap = 4;
        private const string StartMark = "StartMark";
        private const string EndMark = "EndMark";

        private ConcurrentBag<Booking> _bookingStore;
        private SortedList<Booking, string> _timeAxis;
        private object _mutex = new object();

        public BookingStoreProvider()
        {
            _bookingStore = new ConcurrentBag<Booking>();
            var timePeriodList = new SortedList();
            _timeAxis = new SortedList<Booking,string>();
        }

        public async Task<Guid?> AddBooking(Booking booking)
        {
            return await Task.Run(() => 
            {
                if (ExistsBooking(booking))
                    return null;
                var isBookingAvailable = IsTimeAvailableForBooking(booking);
                var newBookingId = Guid.NewGuid();
                booking.BookingId = newBookingId;
                if (isBookingAvailable)
                    _bookingStore.Add(booking);
                return isBookingAvailable?newBookingId:(Guid?)null;
            });
        }

        private bool IsTimeAvailableForBooking(Booking booking)
        {
            int maxOverlap;

            if (ExistsBooking(booking))
                return false;

            var bookingTime = booking.BookingTime;
            var endBookingTime = bookingTime.AddHours(1);
            var endBooking = new Booking() { BookingTime = endBookingTime, Name = booking.Name };

            lock (_mutex)
            {
                _timeAxis.Add(booking, StartMark);
                _timeAxis.Add(endBooking, EndMark);
                maxOverlap = CalculateMaxOverlap();
                if (maxOverlap > MaxBookingOverlap)
                {
                    _timeAxis.Remove(booking);
                    _timeAxis.Remove(endBooking);
                }
            }
            return maxOverlap<=MaxBookingOverlap;         
        }

        private int CalculateMaxOverlap()
        {
            int maxOverlap = 0;
            int currentOpenRangeCount = 0;
            foreach(var timeMark in _timeAxis)
            {
                if (timeMark.Value == StartMark)
                    currentOpenRangeCount++;
                if (timeMark.Value == EndMark)
                    currentOpenRangeCount--;
                maxOverlap = Math.Max(maxOverlap, currentOpenRangeCount);
            }
            return maxOverlap;
        }

        private bool ExistsBooking(Booking booking)
        {
            var existingBooking = _bookingStore.FirstOrDefault(b => b.BookingTime == booking.BookingTime && b.Name == booking.Name);
            return existingBooking != null;
        }
    }
}
