using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Command.Create
{
    public class CreateShowTimeCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateShowTimeCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
        public async Task<ApiResponse<int>> Handle(CreateShowTimeCommandRequest request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Showtime
            {
                ScreenID = request.ScreenID,
                MovieId = request.MovieId,
                DateTime = request.DateTime,
            };
            await _commandUnitofWork.CommandRepository<Domain.Entities.Showtime>().AddAsync(entity);
            var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
            var response = new ApiResponse<int>
            {
                Success = saveResult > 0,
                StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                Data = saveResult,
                Message = saveResult > 0 ? $"ShowTime {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} showtime."
            };
            return response;
        }
    }
}
