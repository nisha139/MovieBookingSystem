using Ardalis.Specification;
using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.Features.Theater.Query.GetTheaterList;
using MovieBooking.Application.Models.Specification;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Query.GetSeatList
{
    public class GetSeatRequestSpec : EntitiesByPaginationFilterSpec<SeatListDto>
    {
        public GetSeatRequestSpec(GetSeatListQuery request)
            : base(request.PaginationFilter) =>
            Query.OrderByDescending(c => c.Status, !request.PaginationFilter.HasOrderBy());
    }
    public class GetSeatListHandler(IQueryUnitOfWork query) : IRequestHandler<GetSeatListQuery, IPagedDataResponse<SeatListDto>>
    {
        private readonly IQueryUnitOfWork _query = query;

        public async Task<IPagedDataResponse<SeatListDto>> Handle(GetSeatListQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetSeatRequestSpec(request);

            return await _query.seatQueryRepository.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
        }
    }

}

