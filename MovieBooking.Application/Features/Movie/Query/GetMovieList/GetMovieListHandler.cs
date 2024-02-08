using Ardalis.Specification;
using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Models.Specification;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Application.Features.Tasks.Query;

namespace MovieBooking.Application.Features.Tasks.Query;

public class GetMovieRequestSpec : EntitiesByPaginationFilterSpec<MovieListDto>
{
    public GetMovieRequestSpec(GetMovieListQuery request)
        : base(request.PaginationFilter) =>
        Query.OrderByDescending(c => c.Title, !request.PaginationFilter.HasOrderBy());
}

public class GetMovieListHandler(IQueryUnitOfWork query) : IRequestHandler<GetMovieListQuery, IPagedDataResponse<MovieListDto>>
{
    private readonly IQueryUnitOfWork _query = query;

    public async Task<IPagedDataResponse<MovieListDto>> Handle(GetMovieListQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetMovieRequestSpec(request);

        return await _query.movieQueryRepository.SearchAsync(spec, request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize, cancellationToken);
    }
}
