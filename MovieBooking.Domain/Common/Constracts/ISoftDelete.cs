using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Common.Constracts
{
    public interface ISoftDelete
    {
        string? DeletedBy { get; set; }
        DateTimeOffset DeletedOn { get; set; }
        bool IsDeleted { get; set; }
    }
}
