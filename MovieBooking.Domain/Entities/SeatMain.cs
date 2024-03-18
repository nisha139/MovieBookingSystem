using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class SeatMain : BaseAuditableEntity
    {
        public Guid ScreenId { get; set; }
        public string Row { get; set; }
        public int Column { get; set; }
        public string Status { get; set; } // Reserved, Available, etc.
        [ForeignKey("ScreenId")]
        public ScreenMain screen { get; set; }
        public List<BookingMain> Bookings { get; set; }

       
    }
}
