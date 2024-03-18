using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Dto
{
    public class SeatMainListDto
    {
        public Guid SeatId { get; set; }
        public Guid ScreenId { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
        public string Status { get; set; } // Reserved, Available, etc.
    }
}
