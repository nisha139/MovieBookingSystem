using MovieBooking.Application.Features.Booking.Dto;

namespace MovieBooking.Application.Services
{
    public interface IBookingDataService
    {
        Task<BookingDetailDto> GetBookingDetailsByIdAsync(Guid bookingId);
    }
}
