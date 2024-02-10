using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Dto
{
    public class BookingDetailDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public Guid ShowtimeID { get; set; }
        public string SeatsBooked { get; set; }

    }
}
