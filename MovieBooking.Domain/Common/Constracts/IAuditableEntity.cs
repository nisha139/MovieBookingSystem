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
        public string? CreatedBy { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
