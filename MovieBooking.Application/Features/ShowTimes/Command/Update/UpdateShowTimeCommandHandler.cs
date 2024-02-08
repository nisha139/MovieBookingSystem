using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Command.Update;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Command.Update
{
    public class UpdateShowTimeCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateShowTimeCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
        public async Task<ApiResponse<string>> Handle(UpdateShowTimeCommandRequest request, CancellationToken cancellationToken)
        {
            var existingShowTime = await _commandUnitofWork.CommandRepository<Domain.Entities.Showtime>().GetByIdAsync(request.ShowTimeId, cancellationToken);

            if (existingShowTime == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = $"ShowTime with ID {request.ShowTimeId} not found.",
                };
            }

            existingShowTime.ScreenID = request.ScreenID;
            existingShowTime.MovieId = request.MovieId;
            existingShowTime.DateTime = request.DateTime;

            await _commandUnitofWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = "ShowTime updated successfully",
                Message = $"ShowTime {ConstantMessages.UpdatedSuccessfully}",
            };

            return response;
        }
    }
}
