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

namespace MovieBooking.Application.Features.Theater.Query.GetTheatersByMovieTitleQuery
{
    public record GetTheatersByMovieTitleQueryRequest(string Title) : IRequest<ApiResponse<List<TheaterDto>>>;

    public class GetTheatersByMovieTitleQueryHandler : IRequestHandler<GetTheatersByMovieTitleQueryRequest, ApiResponse<List<TheaterDto>>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;

        public GetTheatersByMovieTitleQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<TheaterDto>>> Handle(GetTheatersByMovieTitleQueryRequest request, CancellationToken cancellationToken)
        {
            var theaters = await _query.QueryRepository<Domain.Entities.Theater>()
                .GetAllAsync(); // Retrieve all theaters

            var theatersWithMovie = theaters.Where(theater =>
     theater.Screens.Any(screen => screen.Showtimes.Any(showtime =>
         showtime.Movie.Title.ToLower() == request.Title.ToLower())));


            _ = theatersWithMovie ?? throw new NotFoundException("Theaters showing movie ", request.Title);
            //if (!theatersWithMovie.Any())
            //    throw new NotFoundException($"Theaters showing movie '{request.Title}'");

            var theaterDtos = _mapper.Map<List<TheaterDto>>(theatersWithMovie);

            var response = new ApiResponse<List<TheaterDto>>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = theaterDtos,
                Message = $"Theaters showing movie '{request.Title}' found."
            };

            return response;
        }
    }
}
