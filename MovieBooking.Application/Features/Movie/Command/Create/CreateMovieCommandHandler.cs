using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MovieBooking.Application.Features.Movie.Command.Create
{
    public class CreateMovieCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateMovieCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;

        public async Task<ApiResponse<int>> Handle(CreateMovieCommandRequest request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Movie
            {
                Title = request.Title,
                Genre = request.Genre,
                ReleaseDate = request.ReleaseDate,
                Duration = request.Duration,
                IsActive = request.IsActive,
            };
            await _commandUnitofWork.CommandRepository<Domain.Entities.Movie>().AddAsync(entity);
            var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
            var response = new ApiResponse<int>
            {
                Success = saveResult > 0,
                StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                Data = saveResult,
                Message = saveResult > 0 ? $"Movie {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} movie."
            };
            return response;
        }
    }
}
