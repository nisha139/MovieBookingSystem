using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Command.Update
{
    public class UpdateMovieCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateMovieRequestCommand, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;

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

            return response;
        }
    
    }
}
