using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Dto
{
    public class TheaterMainDetailDto
    {
        public int TheaterID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int NoOfScreens { get; set; }
        public string ImageUrl { get; set; }
    }
}
