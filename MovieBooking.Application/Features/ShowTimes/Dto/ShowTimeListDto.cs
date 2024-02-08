using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Dto
{
    public class ShowTimeListDto
    {
        public Guid ShowTimeId {  get; set; }
        public Guid ScreenID { get; set; }
        public Guid MovieId { get; set; }
        public DateTime DateTime { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? CreatedBy { get; set; }

        public DateTimeOffset ModifiedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? ModifiedBy { get; set; }

        public DateTimeOffset DeletedOn { get; set; } = DateTimeOffset.UtcNow;

        public string? DeletedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
