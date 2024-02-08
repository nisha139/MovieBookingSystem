using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Command.Update;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Command.Update
{
    public class UpdateSeatCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateSeatCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
        public async Task<ApiResponse<string>> Handle(UpdateSeatCommandRequest request, CancellationToken cancellationToken)
        {
            var existingSeat = await _commandUnitofWork.CommandRepository<Domain.Entities.Seat>().GetByIdAsync(request.Id, cancellationToken);

            if (existingSeat == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = $"Seat with ID {request.Id} not found.",
                };
            }
            existingSeat.ScreenId = request.ScreenId;
            existingSeat.Column = request.Column;
            existingSeat.Row = request.Row;
            existingSeat.Status = request.Status;

            await _commandUnitofWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = "Seat updated successfully",
                Message = $"Seat {ConstantMessages.UpdatedSuccessfully}",
            };

            return response;
        }
    }
}
