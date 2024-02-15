using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Query.GetTheaterScreensByTitle
{
    public record GetTheaterScreensByTitleQuery(string TheaterTitle) : IRequest<ApiResponse<List<ScreenDto>>>;

    public class GetTheaterScreensByTitleQueryHandler : IRequestHandler<GetTheaterScreensByTitleQuery, ApiResponse<List<ScreenDto>>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;

        public GetTheaterScreensByTitleQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<ScreenDto>>> Handle(GetTheaterScreensByTitleQuery request, CancellationToken cancellationToken)
        {
            var theaters = await _query.QueryRepository<Domain.Entities.Theater>()
                .GetAllAsync(); // Retrieve all theaters

            var theater = theaters.FirstOrDefault(t => t.Name.ToLower() == request.TheaterTitle.ToLower());
            if (theater == null)
                throw new NotFoundException($"Theater with name '{request.TheaterTitle}' not found.", request.TheaterTitle);

            // Ensure theater.Screens is not null before accessing it
            var screens = theater.Screens?.SelectMany(s => s.Showtimes)
                                          .Select(showtime => _mapper.Map<ScreenDto>(showtime.Screen))
                                          .Distinct()
                                          .ToList() ?? new List<ScreenDto>();

            var response = new ApiResponse<List<ScreenDto>>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = screens,
                Message = $"Available screens for theater '{request.TheaterTitle}' found."
            };

            return response;
        }
    }
}
