using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Command.Delete;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Command.Delete
{
    public record DeleteTheaterCommandRequest(Guid id) : IRequest<ApiResponse<string>>;
    public class DeleteTheaterCommandHandler(IQueryUnitOfWork query,
       ICommandUnitOfWork command) : IRequestHandler<DeleteTheaterCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;
        public async Task<ApiResponse<string>> Handle(DeleteTheaterCommandRequest request, CancellationToken cancellationToken)
        {
            var theater = await _query.QueryRepository<Domain.Entities.Theater>().GetByIdAsync(request.id.ToString());
            _ = theater ?? throw new NotFoundException("TheaterId ", request.id);

            _command.CommandRepository<Domain.Entities.Theater>().Remove(theater);
            var result = await _command.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = result.ToString(),
                Message = result > 0 ? $"Theater {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} theater."
            };

            return response;
        }
    }
}
