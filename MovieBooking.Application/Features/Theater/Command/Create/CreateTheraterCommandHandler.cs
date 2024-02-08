using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Command.Create;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Command.Create
{
    public class CreateTheraterCommandHandler(ICommandUnitOfWork command) : IRequestHandler<CreateTheaterCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
        public async Task<ApiResponse<int>> Handle(CreateTheaterCommandRequest request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Theater
            {
                Name = request.Name,
                Location = request.Location,
                NoOfScreen = request.NoOfScreen,
            };
            await _commandUnitofWork.CommandRepository<Domain.Entities.Theater>().AddAsync(entity);
            var saveResult = await _commandUnitofWork.SaveAsync(cancellationToken);
            var response = new ApiResponse<int>
            {
                Success = saveResult > 0,
                StatusCode = saveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                Data = saveResult,
                Message = saveResult > 0 ? $"Theater {ConstantMessages.AddedSuccesfully}" : $"{ConstantMessages.FailedToCreate} theater."
            };
            return response;
        }
    }
}
    