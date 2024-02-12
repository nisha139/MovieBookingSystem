using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBooking.Domain.Entities
{
    public class Showtime : BaseAuditableEntity
    {
        public Guid ScreenID { get; set; }
        public Guid MovieId { get; set; }
        public DateTime DateTime { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        [ForeignKey("ScreenID")]
        public Screen Screen { get; set; }

        public virtual List<Booking> Bookings { get; set; }
       
    }
}
