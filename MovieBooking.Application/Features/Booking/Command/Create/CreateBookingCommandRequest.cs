using MediatR;
using MovieBooking.Application.Features.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Create
{
    public class CreateBookingCommandRequest : IRequest<ApiResponse<int>>
    {
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public Guid ShowtimeID { get; set; }
        public string SeatsBooked { get; set; }
    }
}
