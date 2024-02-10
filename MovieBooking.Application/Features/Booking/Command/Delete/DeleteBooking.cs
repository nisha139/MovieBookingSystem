using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Delete
{
    public record DeleteBookingCommandRequest(Guid id) : IRequest<ApiResponse<string>>;

    public class DeleteBookingCommandHandler(IQueryUnitOfWork query,
        ICommandUnitOfWork command) : IRequestHandler<DeleteBookingCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;

        public async Task<ApiResponse<string>> Handle(DeleteBookingCommandRequest request, CancellationToken cancellationToken)
        {
            var booking = await _query.QueryRepository<Domain.Entities.Booking>().GetByIdAsync(request.id.ToString());
            _ = booking ?? throw new NotFoundException("BookingId ", request.id);

            _command.CommandRepository<Domain.Entities.Booking>().Remove(booking);
            var result = await _command.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = result.ToString(),
                Message = result > 0 ? $"Booking {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} booking."
            };

            return response;
        }
    }
}
