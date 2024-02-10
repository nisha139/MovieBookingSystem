using Ardalis.Specification;
using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Features.Booking.Query.GetBookingList;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Tasks.Query;
using MovieBooking.Application.Models.Specification;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Query.GetBookingList
{
    public class GetBookingRequestSpec : EntitiesByPaginationFilterSpec<BookingListDto>
    {
        public GetBookingRequestSpec(GetBookingListQuery request)
            : base(request.PaginationFilter) =>
            Query.OrderByDescending(c => c.SeatsBooked, !request.PaginationFilter.HasOrderBy());
    }
}

public class GetBookingListHandler(IQueryUnitOfWork query) : IRequestHandler<GetBookingListQuery, IPagedDataResponse<BookingListDto>>
{
    private readonly IQueryUnitOfWork _query = query;

    public async Task<IPagedDataResponse<BookingListDto>> Handle(GetBookingListQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetBookingRequestSpec(request);

        return await _query.bookingQueryRepository.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
    }
}
