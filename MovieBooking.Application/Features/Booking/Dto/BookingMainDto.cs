using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Dto
{
    public class BookingMainDto
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeID { get; set; }
        public decimal Amount { get; set; }
    }
}
