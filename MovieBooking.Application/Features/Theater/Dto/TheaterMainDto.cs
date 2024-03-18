using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Dto
{
    public class TheaterMainDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int NoOfScreen { get; set; }
        public string ImageUrl { get; set; }
        public List<ScreenMainDto> Screens { get; set; }
    }
}
