using Ardalis.Specification;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Screen.Dto;
using MovieBooking.Application.Features.Seat.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Persistence.Repositoris.Seat.Query
{
    public interface ISeatQueryRepository : IQueryRepository<Domain.Entities.Seat>
    {
        Task<List<MovieBooking.Application.Features.Seat.Dto.SeatMainDetailDto>> GetAllSeatMainAsync(CancellationToken cancellationToken);
        Task<MovieBooking.Domain.Entities.Seat> GetByIdAsync(Guid id);
        Task<IPagedDataResponse<SeatListDto>> SearchAsync(ISpecification<SeatListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<IPagedDataResponse<SeatListDto>> SearchBookedSeatAsync(ISpecification<SeatListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);


    }
}
