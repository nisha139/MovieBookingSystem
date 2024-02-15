using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Seat.Query;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Repositories.Base;
using MovieBooking.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Repositories.Seat.Query
{
    public class SeatQueryRepository : QueryRepository<Domain.Entities.Seat>, ISeatQueryRepository
    {
        public SeatQueryRepository(MovieDBContext context) : base(context)
        {
        }

        public async Task<IPagedDataResponse<SeatListDto>> SearchAsync(ISpecification<SeatListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var seatListQuery = context.seats.AsNoTracking()
                .Select(seat => new SeatListDto
                {
                    SeatId = seat.Id,
                    ScreenId = seat.ScreenId,
                    Row = seat.Row,
                    Column = seat.Column,
                    Status = seat.Status,
                });

            var seats = await seatListQuery.ApplySpecification(spec);
            var count = await seatListQuery.ApplySpecificationCount(spec);

            return new PagedApiResponse<SeatListDto>(count, pageNumber, pageSize) { Data = seats };
        }

        public async Task<IPagedDataResponse<SeatListDto>> SearchBookedSeatAsync(ISpecification<SeatListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var seatListQuery = context.seats.AsNoTracking()
                .Where(seat => seat.Status == "booked") // Filter seats with status "booked"
                .Select(seat => new SeatListDto
                {
                    SeatId = seat.Id,
                    ScreenId = seat.ScreenId,
                    Row = seat.Row,
                    Column = seat.Column,
                    Status = seat.Status,
                });

            var seats = await seatListQuery.ApplySpecification(spec);
            var count = await seatListQuery.ApplySpecificationCount(spec);

            return new PagedApiResponse<SeatListDto>(count, pageNumber, pageSize) { Data = seats };
        }

        public async Task<MovieBooking.Domain.Entities.Seat> GetByIdAsync(Guid id)
        {
            return await context.seats.FindAsync(id);
        }


    }
}
