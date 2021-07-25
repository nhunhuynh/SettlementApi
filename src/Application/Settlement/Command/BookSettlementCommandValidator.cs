using FluentValidation;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Settlement.Command
{
    public class BookSettlementCommandValidator: AbstractValidator<BookSettlementCommand>
    {
        public BookSettlementCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required");
            RuleFor(v => v.BookingTime)
                .NotEmpty().WithMessage("Booking time is required")
                .MustAsync(BeValidTime).WithMessage("Booking time is not valid");
        }

        public async Task<bool> BeValidTime(string bookingTime, CancellationToken cancellationToken)
        {
            return await Task.Run(() => 
            {
                var latestBookingTime = DateTime.Today.AddHours(16);
                var isValidTime = true;
                DateTime time;
                
                //check if booking time is in right format
                isValidTime = DateTime.TryParseExact(bookingTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time);
                if (!isValidTime)
                    return isValidTime;

                //check if booking time is within allowed time
                if (time.Hour < 9 || time > latestBookingTime)
                    return false;

                return isValidTime;
            });
        }
    }
}
