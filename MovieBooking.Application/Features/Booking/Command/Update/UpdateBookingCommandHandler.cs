using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Command.Update;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Update
{
    public class UpdateBookingCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateBookingRequestCommand, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;

        public async Task<ApiResponse<string>> Handle(UpdateBookingRequestCommand request, CancellationToken cancellationToken)
        {
            var existingBooking = await _commandUnitofWork.CommandRepository<Domain.Entities.Booking>().GetByIdAsync(request.Id, cancellationToken);

            if (existingBooking == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = $"Booking with ID {request.Id} not found.",
                };
            }

            existingBooking.MovieId = request.MovieId;
            existingBooking.ShowtimeID = request.ShowtimeID;
            existingBooking.UserId = request.UserId;
            existingBooking.SeatsBooked = request.SeatsBooked;

            await _commandUnitofWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = "Booking updated successfully",
                Message = $"Booking {ConstantMessages.UpdatedSuccessfully}",
            };

            return response;
        }

    }
}
