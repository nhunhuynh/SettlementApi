using MediatR;

namespace Application.Settlement.Command
{
    public class BookSettlementCommand : IRequest<BookingResult>
    {
        public string BookingTime { get; set; }
        public string Name { get; set; }
    }
}
