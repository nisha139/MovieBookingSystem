using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Dto
{
    public class BookingListDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public Guid ShowtimeID { get; set; }
        public string SeatsBooked { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
