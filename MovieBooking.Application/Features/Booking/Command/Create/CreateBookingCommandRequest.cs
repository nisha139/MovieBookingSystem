using MediatR;
using MovieBooking.Application.Features.Common;

namespace MovieBooking.Application.Features.Booking.Command.Create
{
    public class CreateBookingCommandRequest : IRequest<ApiResponse<int>>
    {
        //public Guid UserId { get; set; }
        public string ShowtimeID { get; set; }
        public decimal Amount { get; set; }
        public string SeatID { get; set; }
        public string SeatsBooked { get; set; }
        public string PaymentMethodName { get; set; } 
        public string PaymentMethodDescription { get; set; } 
        public decimal PaymentAmount { get; set; } 
    }
}
