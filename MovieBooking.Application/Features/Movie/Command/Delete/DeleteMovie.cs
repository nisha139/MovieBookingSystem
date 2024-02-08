using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Command.Delete
{
    public record DeleteMovieCommandRequest(Guid id) : IRequest<ApiResponse<string>>;

    public class DeleteMovieCommandHandler(IQueryUnitOfWork query,
        ICommandUnitOfWork command) : IRequestHandler<DeleteMovieCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _command = command;
        private readonly IQueryUnitOfWork _query = query;

        public async Task<ApiResponse<string>> Handle(DeleteMovieCommandRequest request, CancellationToken cancellationToken)
        {
            var movie = await _query.QueryRepository<Domain.Entities.Movie>().GetByIdAsync(request.id.ToString());
            _ = movie ?? throw new NotFoundException("MovieId ", request.id);

            _command.CommandRepository<Domain.Entities.Movie>().Remove(movie);
            var result = await _command.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = result.ToString(),
                Message = result > 0 ? $"Movie {ConstantMessages.DeletedSuccessfully}" : $"{ConstantMessages.FailedToCreate} task."
            };

            return response;
        }
    }
}
