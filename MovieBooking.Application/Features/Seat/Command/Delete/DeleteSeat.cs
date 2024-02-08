using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Command.Delete;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Command.Delete
{
    public record DeleteSeatCommandRequest(Guid id) : IRequest<ApiResponse<string>>;
    public class DeleteSeatCommandHandler(IQueryUnitOfWork query,
       ICommandUnitOfWork command) : IRequestHandler<DeleteSeatCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;

        public async Task<ApiResponse<string>> Handle(DeleteSeatCommandRequest request, CancellationToken cancellationToken)
        {
            var seat = await _query.QueryRepository<Domain.Entities.Seat>().GetByIdAsync(request.id.ToString());
            _ = seat ?? throw new NotFoundException("SeatId ", request.id);
            _command.CommandRepository<Domain.Entities.Seat>().Remove(seat);
            var result = await _command.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = result.ToString(),
                Message = result > 0 ? $"Seat {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} seat."
            };

            return response;
        }
}
}
