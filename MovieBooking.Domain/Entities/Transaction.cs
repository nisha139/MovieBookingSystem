using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Domain.Entities
{
    public class Transaction : BaseAuditableEntity
    {
        //public int TransactionID { get; set; }
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public Guid PaymentMethodID { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        [ForeignKey("PaymentMethodID")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
