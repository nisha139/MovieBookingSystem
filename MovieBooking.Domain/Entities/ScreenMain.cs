using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class ScreenMain : BaseAuditableEntity
    {
        public Guid TheaterId { get; set; }
        public int Capacity { get; set; }
        [ForeignKey("TheaterId")]
        public TheaterMain theater { get; set; }

        public List<ShowtimeMain> showtimeMains { get; set; }
        public List<SeatMain> Seats { get; set; }
    }
}
