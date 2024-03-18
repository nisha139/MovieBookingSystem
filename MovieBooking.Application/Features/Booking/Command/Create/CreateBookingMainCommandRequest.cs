using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Create
{
    public class CreateBookingMainCommandRequest : IRequest<ApiResponse<Guid>>
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeID { get; set; }
        public decimal Amount { get; set; }
        public List<Guid> SeatIds { get; set; }
    }
}
