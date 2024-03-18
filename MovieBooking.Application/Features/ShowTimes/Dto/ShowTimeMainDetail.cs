using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Dto
{
    public class ShowTimeMainDetail
    {
        public Guid ShowTimeId { get; set; }
        public Guid ScreenID { get; set; }
        public Guid MovieId { get; set; }
        public DateTime DateTime { get; set; } 
        public string MovieName { get; set; }

    }
}
