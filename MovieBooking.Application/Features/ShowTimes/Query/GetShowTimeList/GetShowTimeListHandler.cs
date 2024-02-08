using Ardalis.Specification;
using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.ShowTimes.Dto;
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

namespace MovieBooking.Application.Features.ShowTimes.Query.GetShowTimeList
{
    public class GetShowTimeRequestSpec : EntitiesByPaginationFilterSpec<ShowTimeListDto>
    {
        public GetShowTimeRequestSpec(GetShowTimeListQuery request)
            : base(request.PaginationFilter) =>
            Query.OrderByDescending(c => c.DateTime, !request.PaginationFilter.HasOrderBy());
    }
    public class GetShowTimeListHandler(IQueryUnitOfWork query) : IRequestHandler<GetShowTimeListQuery, IPagedDataResponse<ShowTimeListDto>>
    {
        private readonly IQueryUnitOfWork _query = query;

        public async Task<IPagedDataResponse<ShowTimeListDto>> Handle(GetShowTimeListQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetShowTimeRequestSpec(request);

            return await _query.showTimeQueryRepostory.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
        }
    }
}
