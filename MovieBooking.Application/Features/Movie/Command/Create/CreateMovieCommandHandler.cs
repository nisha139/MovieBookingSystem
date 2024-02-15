using MediatR;
using Serilog;
using MovieBooking.Application.Behaviours;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Command.Create
{
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitOfWork;
        private readonly Serilog.ILogger _logger;

        public CreateMovieCommandHandler(ICommandUnitOfWork commandUnitOfWork, ILogger logger)
        {
            _commandUnitOfWork = commandUnitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<int>> Handle(CreateMovieCommandRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling CreateMovieCommand");

            var entity = new Domain.Entities.Movie
            {
                Title = request.Title,
                Genre = request.Genre,
                ReleaseDate = request.ReleaseDate,
                Duration = request.Duration,
                IsActive = request.IsActive,
            };

            await _commandUnitOfWork.CommandRepository<Domain.Entities.Movie>().AddAsync(entity);
            var saveResult = await _commandUnitOfWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<int>
            {
                Success = saveResult > 0,
                StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                Data = saveResult,
                Message = saveResult > 0 ? $"Movie {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} movie."
            };

            _logger.Information($"Handled CreateMovieCommand. Success: {response.Success},{request.Title} Movie Created, StatusCode: {response.StatusCode}");

            return response;
        }
    }
}
