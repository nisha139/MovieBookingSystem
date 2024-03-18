using MovieBooking.Domain.Common;
using System;

namespace MovieBooking.Domain.Entities
{
    public class MovieBooking : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string Duration { get; set; }
        public bool IsActive { get; set; }
    }
}
