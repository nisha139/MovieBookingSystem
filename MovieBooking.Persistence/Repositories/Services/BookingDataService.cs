using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Services;
using MovieBooking.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Repositories.Services
{
    public class BookingDataService : IBookingDataService
    {
        private readonly MovieDBContext _dbContext;

        public BookingDataService(MovieDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BookingDetailDto> GetBookingDetailsByIdAsync(Guid bookingId)
        {
            return await _dbContext.Set<BookingDetailDto>()
                .FromSqlRaw("EXECUTE GetBookingDetailsById @BookingId", new SqlParameter("@BookingId", bookingId))
                .FirstOrDefaultAsync();
        }
    }
}
