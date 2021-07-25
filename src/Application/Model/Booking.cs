using System;
using System.Diagnostics.CodeAnalysis;

namespace Application.Model
{
    public class Booking: IComparable<Booking>, IEquatable<Booking>
    {
        public Guid BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public string Name { get; set; }

        public int CompareTo([DisallowNull] Booking other)
        {
            if (this.BookingTime == other.BookingTime)
                return this.Name.CompareTo(other.Name);
            else
                return this.BookingTime.CompareTo(other.BookingTime);
        }

        public bool Equals([DisallowNull] Booking other)
        {
            if (this.BookingTime == other.BookingTime)
                return this.Name.Equals(other.Name);
            else
                return this.BookingTime.Equals(other.BookingTime);
        }
    }
}
