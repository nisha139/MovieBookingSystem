using Ardalis.Specification;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Persistence.Repositoris.Theater.Query
{
    public interface ITheaterQueryRepository : IQueryRepository<Domain.Entities.Theater>
    {
        Task<IPagedDataResponse<TheaterListDto>> SearchAsync(ISpecification<TheaterListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<MovieBooking.Application.Features.Theater.Dto.TheaterMainDto>> GetAllTheaterAsync(CancellationToken cancellationToken);

    }

}
