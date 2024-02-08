using Ardalis.Specification;
using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Screen.Dto;
using MovieBooking.Application.Features.Tasks.Query;
using MovieBooking.Application.Models.Specification;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Query.GetScreenList
{
    public class GetScreenRequestSpec : EntitiesByPaginationFilterSpec<ScreenListDto>
    {
        public GetScreenRequestSpec(GetScreenListQuery request)
            : base(request.PaginationFilter) =>
            Query.OrderByDescending(c => c.Capacity, !request.PaginationFilter.HasOrderBy());
    }

    public class GetScreenListHandler(IQueryUnitOfWork query) : IRequestHandler<GetScreenListQuery, IPagedDataResponse<ScreenListDto>>
    {
        private readonly IQueryUnitOfWork _query = query;

        public async Task<IPagedDataResponse<ScreenListDto>> Handle(GetScreenListQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetScreenRequestSpec(request);

            return await _query.ScreenQueryRepository.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
        }
    }
}
