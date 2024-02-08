using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class Screen : BaseAuditableEntity
    {
        public Guid TheaterId { get; set; }
        public int Capacity { get; set; }
        [ForeignKey("TheaterId")]
        public Theater theater { get; set; }
       
        public List<Showtime> Showtimes { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
