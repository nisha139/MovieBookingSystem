using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Booking.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Domain.Entities;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Repositories.Base;
using MovieBooking.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Repositories.Booking.Query
{
    public class BookingQueryRepository : QueryRepository<MovieBooking.Domain.Entities.Booking>, IBookingQueryRepository
    {
        public BookingQueryRepository(MovieDBContext context) : base(context)
        { }

        public async Task<IPagedDataResponse<BookingListDto>> SearchAsync(ISpecification<BookingListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var bookingListQuery = context.Bookings.AsNoTracking()
                .Select(booking => new BookingListDto
                {
                    Id = booking.Id,
                    UserId = booking.UserId,
                    ShowtimeID = booking.ShowtimeID,
                    SeatsBooked = booking.SeatsBooked,
                    CreatedOn = booking.CreatedOn,
                    CreatedBy = booking.CreatedBy,
                    ModifiedOn = booking.ModifiedOn,
                    ModifiedBy = booking.ModifiedBy
                });
            var bookings = await bookingListQuery.ApplySpecification(spec);

            var count = await bookingListQuery.ApplySpecificationCount(spec);

            return new PagedApiResponse<BookingListDto>(count, pageNumber, pageSize) { Data = bookings };
        }


        public async Task<BookingDetailDto> GetEarliestBookingAsync(string showtimeId, List<Guid> seatIds, CancellationToken cancellationToken)
        {
            // Convert showtimeId to Guid
            Guid showtimeGuid = Guid.Parse(showtimeId);

            // Query the database to find the earliest booking for the provided showtime and seat IDs
            var earliestBooking = context.Bookings
                .Where(b => b.ShowtimeID == showtimeGuid && seatIds.Contains(Guid.Parse(b.SeatsBooked)))
                .OrderBy(b => b.CreatedBy)
                .FirstOrDefault();

            // Mapping the Booking entity to a DTO, adjust this according to your DTO structure
            if (earliestBooking != null)
            {
                return new BookingDetailDto
                {
                    Id = earliestBooking.Id,
                    // Map other properties as needed
                };
            }

            return null; // Return null if no booking is found
        }


    }
}

