using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Command.Delete
{
    public record DeleteScreenCommandRequest(Guid id) : IRequest<ApiResponse<string>>;

    public class DeleteScreenCommandHandler(IQueryUnitOfWork query,
        ICommandUnitOfWork command) : IRequestHandler<DeleteScreenCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;

        public async Task<ApiResponse<string>> Handle(DeleteScreenCommandRequest request, CancellationToken cancellationToken)
        {
            var screen = await _query.QueryRepository<Domain.Entities.Screen>().GetByIdAsync(request.id.ToString());
            _ = screen ?? throw new NotFoundException("ScreenId ", request.id);

            _command.CommandRepository<Domain.Entities.Screen>().Remove(screen);
            var result = await _command.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = result.ToString(),
                Message = result > 0 ? $"Screen {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} screen."
            };

            return response;
        }
    }
}
