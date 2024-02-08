using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class Theater : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public int NoOfScreen { get; set; }
        public List<Screen> Screens { get; set; }

    }
}
