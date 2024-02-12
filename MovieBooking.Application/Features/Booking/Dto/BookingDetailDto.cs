using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using System;

namespace MovieBooking.Application.Features.Booking.Dto
{
    public class BookingDetailDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public Guid ShowtimeID { get; set; }
        public string SeatsBooked { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
        public TransactionDto Transaction { get; set; }
        public string MovieName { get; set; }
        public ShowtimeDto Showtime { get; set; }
        public SeatDetailDto Seat { get; set; } // Include SeatDetailDto for seat information
        public ScreenDto Screen { get; set; } 
    }

    public class PaymentMethodDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        // Add other properties as needed
    }

    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        // Add other properties as needed
    }
}
