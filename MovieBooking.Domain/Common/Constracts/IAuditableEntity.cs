using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Common.Constracts
{
    public interface IAuditableEntity
    {
        public DateTimeOffset CreatedOn { get; set; }
        //DateTimeOffset instead of DateTime allows
        //for handling time zones and provides better accuracy
        //when dealing with time-sensitive operations.
        //DateTime is able to reflect only Coordinated Universal Time (UTC) and the system's local time zone
        public string? CreatedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
