using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class ShowtimeMain : BaseAuditableEntity
    {
        public Guid ScreenID { get; set; }
        public Guid MovieId { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey("MovieId")]
        public MovieBooking Movie { get; set; }

        [ForeignKey("ScreenID")]
        public ScreenMain Screen { get; set; }

        public virtual List<BookingMain> Bookings { get; set; }
    }
}
