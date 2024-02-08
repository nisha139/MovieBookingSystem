using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Command.Update;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Command.Update
{
    public class UpdateTheaterCommandHandler(ICommandUnitOfWork command) : IRequestHandler<UpdateTheaterRequestCommand, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitofWork = command;
        public async Task<ApiResponse<string>> Handle(UpdateTheaterRequestCommand request, CancellationToken cancellationToken)
        {
            var existingTheater = await _commandUnitofWork.CommandRepository<Domain.Entities.Theater>().GetByIdAsync(request.Id, cancellationToken);

            if (existingTheater == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = $"Theater with ID {request.Id} not found.",
                };
            }

            existingTheater.Name = request.Name;
            existingTheater.Location = request.Location;
            existingTheater.NoOfScreen = request.NoOfScreen;

            await _commandUnitofWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = "Theater updated successfully",
                Message = $"Theater {ConstantMessages.UpdatedSuccessfully}",
            };

            return response;
        }
    }
}
