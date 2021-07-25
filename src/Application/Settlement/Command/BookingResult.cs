using Application.Model;
using System;

namespace Application.Settlement.Command
{
    public class BookingResult
    {
        public Guid? BookingId { get; set; }
        public BookingStatus Outcome { get; set; }
    }
}
