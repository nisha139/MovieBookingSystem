using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Models.Specification.Filters;

namespace MovieBooking.Application.Features.Tasks.Query;

public sealed record GetMovieListQuery : IRequest<IPagedDataResponse<MovieListDto>>
{
    public PaginationFilter PaginationFilter { get; set; } = default!;
}
