using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.ShowTime.Query;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.ShowTimes.Dto;
using MovieBooking.Domain.Entities;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Repositories.Base;
using MovieBooking.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Repositories.ShowTIme.Query
{
    public class ShowTimeRepository : QueryRepository<Domain.Entities.Showtime>, IShowTimeQueryRepostory
    {

        public ShowTimeRepository(MovieDBContext context) : base(context)
        {
        }
        public async Task<IPagedDataResponse<ShowTimeListDto>> SearchAsync(ISpecification<ShowTimeListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var showTimeListQuery = context.showtimes.AsNoTracking()
                .Select(showtime => new ShowTimeListDto
                {
                    ShowTimeId = showtime.Id,
                    ScreenID = showtime.ScreenID,
                    MovieId = showtime.MovieId,
                    DateTime = showtime.DateTime,
                    CreatedOn = showtime.CreatedOn,
                    CreatedBy = showtime.CreatedBy,
                    ModifiedOn = showtime.ModifiedOn,
                    ModifiedBy = showtime.ModifiedBy
                });
            var showtimes = await showTimeListQuery.ApplySpecification(spec);

            var count = await showTimeListQuery.ApplySpecificationCount(spec);

            return new PagedApiResponse<ShowTimeListDto>(count, pageNumber, pageSize) { Data = showtimes };
        }

        public List<ShowTimeDetailDto> GetAvailableShowTimes(Guid movieId, Guid theaterId)
        {
            var availableShowTimes = context.showtimes
                .Where(s => s.MovieId == movieId && s.Screen.theater.Id == theaterId)
                .Select(s => new ShowTimeDetailDto
                {
                    ShowTimeId = s.Id,
                    ScreenID = s.ScreenID,
                    MovieId = s.MovieId,
                    DateTime = s.DateTime
                })
                .ToList();

            return availableShowTimes;
        }


        public List<ShowTimeDetailDto> GetAvailableShowTimesForScreen(Guid movieId, Guid theaterId, Guid screenId) 
        {
            var availableShowTimes = context.showtimes
                .Where(s => s.MovieId == movieId && s.Screen.theater.Id == theaterId && s.ScreenID == screenId) 
                .Select(s => new ShowTimeDetailDto
                {
                    ShowTimeId = s.Id,
                    ScreenID = s.ScreenID,
                    MovieId = s.MovieId,
                    DateTime = s.DateTime
                })
                .ToList();

            return availableShowTimes;
        }

    }
}
