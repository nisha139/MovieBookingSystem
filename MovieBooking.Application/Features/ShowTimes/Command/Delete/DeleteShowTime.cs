using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Command.Delete
{
    public record DeleteShowTimeCommandRequest(Guid id) : IRequest<ApiResponse<string>>;
    public class DeleteShowTimeCommandHandler(IQueryUnitOfWork query,
       ICommandUnitOfWork command) : IRequestHandler<DeleteShowTimeCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;
        public async Task<ApiResponse<string>> Handle(DeleteShowTimeCommandRequest request, CancellationToken cancellationToken)
        {
            var showtime = await _query.QueryRepository<Domain.Entities.Showtime>().GetByIdAsync(request.id.ToString());
            _ = showtime ?? throw new NotFoundException("ShowTimeId ", request.id);

            _command.CommandRepository<Domain.Entities.Showtime>().Remove(showtime);
            var result = await _command.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = result.ToString(),
                Message = result > 0 ? $"ShowTime {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} showtime."
            };

            return response;
        }
    }
}
