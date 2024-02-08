using MovieBooking.Domain.Common;
using MovieBooking.Domain.Common.Constracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class Log : BaseAuditableEntity
    {
       // public int LogID { get; set; }
        public Guid UserID { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        //public User User { get; set; }
    }
}
