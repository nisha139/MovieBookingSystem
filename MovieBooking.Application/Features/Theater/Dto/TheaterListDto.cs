using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Dto
{
    public class TheaterListDto
    {
        public Guid TheaterID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int NoOfScreens { get; set; }    
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? CreatedBy { get; set; }

        public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? ModifiedBy { get; set; }

        public DateTimeOffset DeletedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
