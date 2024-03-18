using MediatR;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Features.Movie.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Queries
{
    public class GetAllMoviesQuery : IRequest<List<MovieBooking.Application.Features.Movie.Dto.MovieBooking>>
    {
    }

    public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, List<MovieBooking.Application.Features.Movie.Dto.MovieBooking>>
    {
        private readonly IMovieQueryRepository _movieQueryRepository;

        public GetAllMoviesQueryHandler(IMovieQueryRepository movieQueryRepository)
        {
            _movieQueryRepository = movieQueryRepository;
        }

        public async Task<List<MovieBooking.Application.Features.Movie.Dto.MovieBooking>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            return await _movieQueryRepository.GetAllMoviesAsync(cancellationToken);
        }
    }
}
