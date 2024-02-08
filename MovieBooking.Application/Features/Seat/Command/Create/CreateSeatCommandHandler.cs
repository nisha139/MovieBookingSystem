using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Command.Create
{
    public class CreateSeatCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateSeatCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
        public async Task<ApiResponse<int>> Handle(CreateSeatCommandRequest request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Seat
            {
                ScreenId = request.ScreenId,
                Row = request.Row,
                Column = request.Column,
                Status= request.Status,
            };
            await _commandUnitofWork.CommandRepository<Domain.Entities.Seat>().AddAsync(entity);
            var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
            var response = new ApiResponse<int>
            {
                Success = saveResult > 0,
                StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                Data = saveResult,
                Message = saveResult > 0 ? $"Seat {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} seat."
            };
            return response;
        }
    }
}
