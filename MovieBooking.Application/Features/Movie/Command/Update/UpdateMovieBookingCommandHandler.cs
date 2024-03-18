using MediatR;
using Microsoft.Extensions.Logging;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Command.Update
{
    public class UpdateMovieBookingCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateMovieBookingCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
       

        public async Task<ApiResponse<string>> Handle(UpdateMovieBookingCommandRequest request, CancellationToken cancellationToken)
        {
            var existingMovie = await _commandUnitofWork.CommandRepository<Domain.Entities.MovieBooking>().GetByIdAsync(request.Id, cancellationToken);

            if (existingMovie == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = $"Movie with ID {request.Id} not found.",
                };
            }
            existingMovie.ImageUrl = request.ImageUrl;
            existingMovie.Name = request.Name;
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
           // _logger.Information($"Movie Updated from Old Values : {existingMovie.Name} to {request.Name}");

            return response;
        }

    }
}
