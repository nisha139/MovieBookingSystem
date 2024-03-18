using MediatR;
using Microsoft.Extensions.Logging;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Command.Create
{
    public class CreateMovieBookingCommandHandler : IRequestHandler<CreateMovieBookingCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitOfWork;
        

        public CreateMovieBookingCommandHandler(ICommandUnitOfWork commandUnitOfWork)
        {
            _commandUnitOfWork = commandUnitOfWork;
          
        }

        public async Task<ApiResponse<int>> Handle(CreateMovieBookingCommandRequest request, CancellationToken cancellationToken)
        {
          //  _logger.Information("Handling CreateMovieCommand");

            var entity = new Domain.Entities.MovieBooking
            {
                Name = request.Name,
                Genre = request.Genre,
                ReleaseDate = request.ReleaseDate,
                Duration = request.Duration,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive,
            };

            await _commandUnitOfWork.CommandRepository<Domain.Entities.MovieBooking>().AddAsync(entity);
            var saveResult = await _commandUnitOfWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<int>
            {
                Success = saveResult > 0,
                StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                Data = saveResult,
                Message = saveResult > 0 ? $"Movie {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} movie."
            };

          //  _logger.Information($"Handled CreateMovieCommand. Success: {response.Success},{request.Name} Movie Created, StatusCode: {response.StatusCode}");

            return response;
        }
    }
}
