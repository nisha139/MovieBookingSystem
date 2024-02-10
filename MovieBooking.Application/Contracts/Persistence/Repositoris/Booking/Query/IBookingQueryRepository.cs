using Ardalis.Specification;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Features.Movie.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Persistence.Repositoris.Booking.Query
{
    public interface IBookingQueryRepository
    {
        Task<IPagedDataResponse<BookingListDto>> SearchAsync(ISpecification<BookingListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
