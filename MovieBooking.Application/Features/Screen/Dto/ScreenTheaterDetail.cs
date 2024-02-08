using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Dto
{
    public class ScreenTheaterDetail
    {
        public Guid Id { get; set; }
        public Guid TheaterId { get; set; }
        public string TheaterName { get; set; } // Add TheaterName property
        public int Capacity { get; set; }
    }
}
