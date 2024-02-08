using Ardalis.Specification;
using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Tasks.Query;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.Models.Specification;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Query.GetTheaterList
{
    public class GetTheaterRequestSpec : EntitiesByPaginationFilterSpec<TheaterListDto>
    {
        public GetTheaterRequestSpec(GetTheaterListQuery request)
            : base(request.PaginationFilter) =>
            Query.OrderByDescending(c => c.Name, !request.PaginationFilter.HasOrderBy());
    }
    public class GetTheaterListHandler(IQueryUnitOfWork query) : IRequestHandler<GetTheaterListQuery, IPagedDataResponse<TheaterListDto>>
    {
        private readonly IQueryUnitOfWork _query = query;

        public async Task<IPagedDataResponse<TheaterListDto>> Handle(GetTheaterListQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetTheaterRequestSpec(request);

            return await _query.theaterQueryRepository.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
        }
    }
}
