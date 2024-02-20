using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Screen.Query;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Screen.Dto;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Repositories.Base;
using MovieBooking.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Repositories.Screen.Query
{
    public class ScreenQueryRepository : QueryRepository<Domain.Entities.Screen>, IScreenQueryRepository
    {
        public ScreenQueryRepository(MovieDBContext context) : base(context)
        { }
        public async Task<IPagedDataResponse<ScreenListDto>> SearchAsync(ISpecification<ScreenListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var screenlistquery = context.Screens.AsNoTracking()
                .Select(screen => new ScreenListDto
                {
                    Id = screen.Id,
                    Capacity = screen.Capacity,
                });
            var movies = await screenlistquery.ApplySpecification(spec);

            var count = await screenlistquery.ApplySpecificationCount(spec);

            return new PagedApiResponse<ScreenListDto>(count, pageNumber, pageSize) { Data = movies };
        }

        public async Task LoadScreensForTheaterAsync(Domain.Entities.Theater theater)
        {
            // Assuming you have a navigation property named 'Theater' in your Screen entity
            await context.Entry(theater)
                .Collection(t => t.Screens)
                .LoadAsync();
        }
    }
}
