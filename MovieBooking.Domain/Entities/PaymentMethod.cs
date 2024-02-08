using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class PaymentMethod : BaseAuditableEntity
    {
        //public int PaymentMethodID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
