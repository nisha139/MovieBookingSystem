using MediatR;
using Microsoft.Extensions.Logging;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.UnitOfWork;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieBooking.Application.Features.Theater.Query.GetTheatersByMovieTitleQuery
{
    public record GetTheatersByMovieTitleQueryRequest(string Title) : IRequest<ApiResponse<List<TheaterDto>>>;

    public class GetTheatersByMovieTitleQueryHandler : IRequestHandler<GetTheatersByMovieTitleQueryRequest, ApiResponse<List<TheaterDto>>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        public GetTheatersByMovieTitleQueryHandler(IQueryUnitOfWork query, IMapper mapper, Serilog.ILogger logger)
        {
            _query = query;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<TheaterDto>>> Handle(GetTheatersByMovieTitleQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var theaters = await _query.QueryRepository<Domain.Entities.Theater>().GetAllAsync();
                var theatersWithMovie = theaters.Where(theater =>
                    theater.Screens.Any(screen => screen.Showtimes.Any(showtime =>
                        showtime.Movie.Title.ToLower() == request.Title.ToLower())));

                if (!theatersWithMovie.Any())
                    throw new NotFoundException($"Theaters showing movie '{request.Title}' not found", request.Title);


                var theaterDtos = _mapper.Map<List<TheaterDto>>(theatersWithMovie);
                var response = new ApiResponse<List<TheaterDto>>
                {
                    Success = true,
                    StatusCode = HttpStatusCodes.OK,
                    Data = theaterDtos,
                    Message = $"Theaters showing movie '{request.Title}' found."
                };

                _logger.Information($"Searching Theaters for : {request.Title} Movie");
                _logger.Information($"Available Theaters for {request.Title} is: {JsonSerializer.Serialize(theaterDtos)}");

                // Log the event
                //var logEvent = new LogEvent
                //{
                //    UserID = Guid.NewGuid(), // Assuming you have user ID
                //    Action = $"Searching Theaters for movie: {request.Title}",
                //    Timestamp = DateTime.Now
                //};
                //await _mediator.Send(new LogEventCommand { LogEvent = logEvent });

                return response;
            }
            catch (NotFoundException ex)
            {
                _logger.Warning(ex.Message);
                return new ApiResponse<List<TheaterDto>>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = ex.Message,
                    Data = new List<TheaterDto>()
                };
            }

            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while searching theaters for movie: {request.Title}");
                return new ApiResponse<List<TheaterDto>>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.InternalServerError,
                    Message = $"An error occurred while searching theaters for movie: {request.Title}",
                    Data = new List<TheaterDto>()
                };
            }
        }
    }
}
