using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.UnitOfWork;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Command.Update
{
    public class UpdateMovieCommandHandler(ICommandUnitOfWork command,ILogger logger) : IRequestHandler<UpdateMovieRequestCommand, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
        private readonly Serilog.ILogger _logger = logger;

        public async Task<ApiResponse<string>> Handle(UpdateMovieRequestCommand request, CancellationToken cancellationToken)
        {
            var existingMovie = await _commandUnitofWork.CommandRepository<Domain.Entities.Movie>().GetByIdAsync(request.Id, cancellationToken);

            if (existingMovie == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = $"Movie with ID {request.Id} not found.",
                };
            }

            existingMovie.Title = request.Title;
            existingMovie.Genre = request.Genre;
            existingMovie.ReleaseDate = request.ReleaseDate;
            existingMovie.Duration = request.Duration;
            existingMovie.IsActive = request.IsActive;

            await _commandUnitofWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = "Movie updated successfully",
                Message = $"Movie {ConstantMessages.UpdatedSuccessfully}",
            };
            _logger.Information($"Movie Updated from Old Values : {existingMovie.Title} to {request.Title}");

            return response;
        }
    
    }
}
