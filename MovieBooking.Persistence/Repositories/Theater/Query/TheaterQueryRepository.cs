using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Theater.Query;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Repositories.Base;
using MovieBooking.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Repositories.Theater.Query
{
    public class TheaterQueryRepository : QueryRepository<Domain.Entities.Theater>, ITheaterQueryRepository
    {
        public TheaterQueryRepository(MovieDBContext context) : base(context)
        { }

        public async Task<List<MovieBooking.Application.Features.Theater.Dto.TheaterMainDto>> GetAllTheaterAsync(CancellationToken cancellationToken)
        {
            var theaters = await context.theaterMains
                .Select(theater => new MovieBooking.Application.Features.Theater.Dto.TheaterMainDto
                {
                    Id = theater.Id,
                    Name = theater.Name,
                    Location= theater.Location,
                    NoOfScreen= theater.NoOfScreen,
                    ImageUrl = theater.ImageUrl,
                })
                .ToListAsync(cancellationToken);

            return theaters;
        }

        public async Task<IPagedDataResponse<TheaterListDto>> SearchAsync(ISpecification<TheaterListDto> spec, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var theaterListQuery = context.Theater.AsNoTracking()
                .Select(theater => new TheaterListDto
                {
                    TheaterID = theater.Id,
                    Name = theater.Name,
                    Location = theater.Location,
                    NoOfScreens = theater.NoOfScreen,
                    CreatedOn = theater.CreatedOn,
                    CreatedBy = theater.CreatedBy,
                    ModifiedOn = theater.ModifiedOn,
                    ModifiedBy = theater.ModifiedBy
                });
            var theaters = await theaterListQuery.ApplySpecification(spec);

            var count = await theaterListQuery.ApplySpecificationCount(spec);

            return new PagedApiResponse<TheaterListDto>(count, pageNumber, pageSize) { Data = theaters };
            {
            }
        }
    }
}
