using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Persistence.Database;
using MovieBooking.Domain.Entities;
using MovieBooking.Persistence.Repositories.Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Persistence.Services;
using System.Diagnostics;

namespace MovieBooking.Persistence.Repositories.Movie.Query
{
    public class MovieQueryRepository : QueryRepository<Domain.Entities.Movie>, IMovieQueryRepository
    {
        public MovieQueryRepository(MovieDBContext context) : base(context)
        { }

        public async Task<IPagedDataResponse<MovieListDto>> SearchAsync(ISpecification<MovieListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var movieListQuery = context.Movies.AsNoTracking()
                .Select(movie => new MovieListDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Genre = movie.Genre,
                    ReleaseDate = movie.ReleaseDate,
                    Duration = movie.Duration,
                    IsActive = movie.IsActive,
                    CreatedOn = movie.CreatedOn,
                    CreatedBy = movie.CreatedBy,
                    ModifiedOn = movie.ModifiedOn,
                    ModifiedBy = movie.ModifiedBy
                });
            var movies = await movieListQuery.ApplySpecification(spec);

            var count = await movieListQuery.ApplySpecificationCount(spec);

            return new PagedApiResponse<MovieListDto>(count, pageNumber, pageSize) { Data = movies };
        }
    }
}
