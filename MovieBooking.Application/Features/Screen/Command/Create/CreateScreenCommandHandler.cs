using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Command.Create
{
    public class CreateScreenCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateScreenCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;

        public async Task<ApiResponse<int>> Handle(CreateScreenCommandRequest request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Screen
            {
                TheaterId= request.TheaterId,
                Capacity = request.Capacity,
               
            };
            await _commandUnitofWork.CommandRepository<Domain.Entities.Screen>().AddAsync(entity);
            var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
            var response = new ApiResponse<int>
            {
                Success = saveResult > 0,
                StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                Data = saveResult,
                Message = saveResult > 0 ? $"Screen {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} screen."
            };
            return response;
        }

    }
}
