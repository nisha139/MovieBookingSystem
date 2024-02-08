using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class Seat : BaseAuditableEntity
    {
        //public int SeatID { get; set; }
        public Guid ScreenId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Status { get; set; } // Reserved, Available, etc.
        [ForeignKey("ScreenId")]
        public Screen screen { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
