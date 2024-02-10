using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Create
{
    public class CreateBookingCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateBookingCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;

        public async Task<ApiResponse<int>> Handle(CreateBookingCommandRequest request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Booking
            {
                UserId = request.UserId,
                MovieId = request.MovieId,
                ShowtimeID = request.ShowtimeID,
                SeatsBooked = request.SeatsBooked
            };
            await _commandUnitofWork.CommandRepository<Domain.Entities.Booking>().AddAsync(entity);
            var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
            var response = new ApiResponse<int>
            {
                Success = saveResult > 0,
                StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                Data = saveResult,
                Message = saveResult > 0 ? $"Booking {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} booking."
            };
            return response;
        }
    }
}
