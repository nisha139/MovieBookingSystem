using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class BookingMain : BaseAuditableEntity
    {
        public Guid UserId { get; set; }
        public Guid ShowtimeID { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("ShowtimeID")]
        public ShowtimeMain Showtime { get; set; }
        public List<MovieBooking> movies { get; set; }
    }
}
