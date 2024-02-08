using Ardalis.Specification;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query
{
    public interface IMovieQueryRepository : IQueryRepository<Domain.Entities.Movie>
    {
        Task<IPagedDataResponse<MovieListDto>> SearchAsync(ISpecification<MovieListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
}
