using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Update
{
    public class UpdateBookingRequestCommand : IRequest<ApiResponse<string>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public Guid ShowtimeID { get; set; }
        public string SeatsBooked { get; set; }
        public Guid PaymentMethodId { get; set; }
        public decimal TransactionAmount { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodDescription { get; set; }
    }
}
