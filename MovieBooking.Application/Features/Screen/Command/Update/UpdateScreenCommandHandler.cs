using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Command.Update;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Command.Update
{
    public class UpdateScreenCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateScreenRequestCommand, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
        public async Task<ApiResponse<string>> Handle(UpdateScreenRequestCommand request, CancellationToken cancellationToken)
        {
            var existingScreen = await _commandUnitofWork.CommandRepository<Domain.Entities.Screen>().GetByIdAsync(request.Id, cancellationToken);

            if (existingScreen == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = $"Screen with ID {request.Id} not found.",
                };
            }

            existingScreen.Capacity = request.Capacity;

            await _commandUnitofWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = "Screen updated successfully",
                Message = $"Screen {ConstantMessages.UpdatedSuccessfully}",
            };

            return response;
        }
    }
}
