using Ardalis.Specification;
using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Features.Seat.Query.GetBookedSeatList;
using MovieBooking.Application.Features.Seat.Query.GetSeatList;
using MovieBooking.Application.Models.Specification;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Query.GetBookedSearchList
{
    public class GetBookedSeatRequestSpec : EntitiesByPaginationFilterSpec<SeatListDto>
    {
        public GetBookedSeatRequestSpec(GetBookedSeatListQuery request)
            : base(request.PaginationFilter) =>
            Query.OrderByDescending(c => c.Status, !request.PaginationFilter.HasOrderBy());
    }

    public class GetBookedSeatListHandler : IRequestHandler<GetBookedSeatListQuery, IPagedDataResponse<SeatListDto>>
    {
        private readonly IQueryUnitOfWork _query;

        public GetBookedSeatListHandler(IQueryUnitOfWork query)
        {
            _query = query;
        }

        public async Task<IPagedDataResponse<SeatListDto>> Handle(GetBookedSeatListQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetBookedSeatRequestSpec(request);

            return await _query.seatQueryRepository.SearchBookedSeatAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
        }
    }
}
