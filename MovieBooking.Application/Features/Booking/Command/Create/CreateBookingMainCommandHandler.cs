using MediatR;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Create
{
    public class CreateBookingMainCommandHandler : IRequestHandler<CreateBookingMainCommandRequest, ApiResponse<Guid>>
    {
        private readonly ICommandUnitOfWork _commandUnitOfWork;
        private readonly IQueryUnitOfWork _queryUnitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateBookingMainCommandHandler(ICommandUnitOfWork commandUnitOfWork, IQueryUnitOfWork queryUnitOfWork, ICurrentUserService currentUserService)
        {
            _commandUnitOfWork = commandUnitOfWork;
            _queryUnitOfWork = queryUnitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateBookingMainCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Begin transaction
                await _commandUnitOfWork.BeginTransaction();

                // Get the seat entities for the requested seats
                var seatsToBook = (await _queryUnitOfWork.QueryRepository<SeatMain>()
                    .GetAllAsync())
                    .Where(seat => request.SeatIds.Contains(seat.Id))
                    .ToList();

                // Check if any of the seats are already booked
                if (seatsToBook.Any(seat => seat.Status != "Available"))
                {
                    // Rollback transaction
                    await _commandUnitOfWork.RollbackTransactionAsync();

                    return new ApiResponse<Guid>
                    {
                        Success = false,
                        StatusCode = HttpStatusCodes.Conflict, // HTTP status code 409 Conflict
                        Data = Guid.Empty,
                        Message = "One or more seats are not available."
                    };
                }

                // Convert the string UserId to Guid
                Guid userId;
                if (!Guid.TryParse(_currentUserService.UserId, out userId))
                {
                    // Handle invalid UserId format
                    return new ApiResponse<Guid>
                    {
                        Success = false,
                        StatusCode = HttpStatusCodes.BadRequest,
                        Data = Guid.Empty,
                        Message = "Invalid UserId format."
                    };
                }

                // Create a new Booking entity
                var bookingEntity = new BookingMain
                {
                    UserId = userId,
                    ShowtimeID = request.ShowtimeID,
                    Amount = request.Amount
                };

                // Add the booking entity to the repository
                await _commandUnitOfWork.CommandRepository<BookingMain>().AddAsync(bookingEntity);

                // Update seat statuses to "Booked"
                foreach (var seat in seatsToBook)
                {
                    seat.Status = "Booked";
                    _commandUnitOfWork.CommandRepository<SeatMain>().Update(seat);
                }

                // Commit transaction
                await _commandUnitOfWork.CommitTransactionAsync();

                // Save changes to get the booking ID
                await _commandUnitOfWork.SaveAsync(cancellationToken);

                return new ApiResponse<Guid>
                {
                    Success = true,
                    StatusCode = HttpStatusCodes.Created,
                    Data = bookingEntity.Id,
                    Message = "Booking added successfully"
                };
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of exception
                await _commandUnitOfWork.RollbackTransactionAsync();

                // Handle exception appropriately
                return new ApiResponse<Guid>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.InternalServerError,
                    Data = Guid.Empty,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
