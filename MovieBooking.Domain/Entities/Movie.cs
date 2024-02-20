using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class Movie : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Genre { get; set; }  
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public List<Showtime> Showtimes { get; set; }

        public Movie()
        {
            Genre = string.Empty;  
        }
    }
}
