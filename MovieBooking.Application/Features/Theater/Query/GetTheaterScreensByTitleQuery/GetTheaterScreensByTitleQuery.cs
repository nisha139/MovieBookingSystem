using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.UnitOfWork;
using Serilog;

namespace MovieBooking.Application.Features.Theater.Query.GetTheaterScreensByTitle
{
    public record GetTheaterWithScreensQueryRequest(Guid TheaterId) : IRequest<ApiResponse<TheaterDto>>;

    public class GetTheaterWithScreensQueryHandler : IRequestHandler<GetTheaterWithScreensQueryRequest, ApiResponse<TheaterDto>>
    {
        private readonly IQueryUnitOfWork _queryUnitOfWork;
        private readonly ILogger _logger;

        public GetTheaterWithScreensQueryHandler(IQueryUnitOfWork queryUnitOfWork, ILogger logger)
        {
            _queryUnitOfWork = queryUnitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<TheaterDto>> Handle(GetTheaterWithScreensQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var theater = await _queryUnitOfWork.theaterQueryRepository.GetByIdAsync(request.TheaterId.ToString());

                if (theater == null)
                    throw new NotFoundException(nameof(Theater), request.TheaterId);

                // Load screens for the theater
                await _queryUnitOfWork.ScreenQueryRepository.LoadScreensForTheaterAsync(theater);

                var theaterDto = new TheaterDto
                {
                    Id = theater.Id,
                    Name = theater.Name,
                    Location = theater.Location,
                    NoOfScreen = theater.NoOfScreen,
                    Screens = theater.Screens.Select(screen => new ScreenDto
                    {
                        Id = screen.Id,
                        Capacity = screen.Capacity
                    }).ToList()
                };

                return new ApiResponse<TheaterDto>
                {
                    Success = true,
                    StatusCode = HttpStatusCodes.OK,
                    Data = theaterDto,
                    Message = "Screen details retrieved successfully."
                };
            }
            catch (NotFoundException ex)
            {
                _logger.Warning(ex.Message);
                return new ApiResponse<TheaterDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = ex.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while retrieving theater details for TheaterId: {request.TheaterId}");
                return new ApiResponse<TheaterDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.InternalServerError,
                    Message = $"An error occurred while retrieving theater details.",
                    Data = null
                };
            }
        }
    }
}
