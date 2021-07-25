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
        
        private ConcurrentBag<Booking> _bookingStore;
        private SortedList<Booking, int> _timeAxis;
        private object _mutex = new object();

        public BookingStoreProvider()
        {
            _bookingStore = new ConcurrentBag<Booking>();
            var timePeriodList = new SortedList();
            _timeAxis = new SortedList<Booking,int>();
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
                //mark entering a new time period
                if (_timeAxis.ContainsKey(booking))
                    _timeAxis[booking] = _timeAxis[booking] + 1;
                else
                    _timeAxis[booking] = 1;

                //mark ending an existing time period
                if (_timeAxis.ContainsKey(endBooking))
                    _timeAxis[endBooking] = _timeAxis[endBooking] - 1;
                else
                    _timeAxis[endBooking] = -1;

                maxOverlap = CalculateMaxOverlap();
                if (maxOverlap > MaxBookingOverlap)
                {
                    if (_timeAxis[booking] == 1)
                        _timeAxis.Remove(booking);
                    else
                        _timeAxis[booking] = _timeAxis[booking] - 1;

                    if (_timeAxis[endBooking] == 1)
                        _timeAxis.Remove(endBooking);
                    else
                        _timeAxis[endBooking] = _timeAxis[endBooking] + 1;                    
                }
            }
            return maxOverlap<=MaxBookingOverlap;         
        }

        private int CalculateMaxOverlap()
        {
            int maxOverlap = 0;
            int currentOpenRangeCount = 0;
            for(int i=0;i<_timeAxis.Keys.Count;i++)
            {              
                currentOpenRangeCount += _timeAxis[_timeAxis.Keys[i]];  
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
