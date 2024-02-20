using Ardalis.Specification;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Screen.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Persistence.Repositoris.Screen.Query
{
    public interface IScreenQueryRepository : IQueryRepository<Domain.Entities.Screen>
    {
        Task<IPagedDataResponse<ScreenListDto>> SearchAsync(ISpecification<ScreenListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task LoadScreensForTheaterAsync(Domain.Entities.Theater theater);
    }
}
