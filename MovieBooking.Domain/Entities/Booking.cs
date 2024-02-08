    using MovieBooking.Domain.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Transactions;

    namespace MovieBooking.Domain.Entities
    {
        public class Booking : BaseAuditableEntity
        {
            public Guid UserId { get; set; }
            public Guid MovieId { get; set; }
            public Guid ShowtimeID { get; set; }
            public string SeatsBooked { get; set; }
       
            [ForeignKey("MovieId")]
            public Movie Movie { get; set; }

            [ForeignKey("ShowtimeID")]
            public Showtime Showtime { get; set; }

           public List<Transaction> Transactions { get; set; }
            public List<Movie> movies { get; set; }
        }
    }
