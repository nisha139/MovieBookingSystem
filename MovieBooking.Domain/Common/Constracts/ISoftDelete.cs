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

//Soft deletion is a common pattern in applications
//where it is necessary to keep a record of deleted entities,
//but not display them in the active data. 
