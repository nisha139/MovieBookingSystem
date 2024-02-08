using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Dto
{
    public class SeatListDto
    {
        public Guid SeatId { get; set; }
        public Guid ScreenId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Status { get; set; } // Reserved, Available, etc.
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? CreatedBy { get; set; }

        public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? ModifiedBy { get; set; }

        public DateTimeOffset DeletedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
