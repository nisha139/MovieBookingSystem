using MediatR;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Create
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommandRequest, ApiResponse<int>>
    {
        private readonly ICommandUnitOfWork _commandUnitOfWork;
        private readonly IQueryUnitOfWork _queryUnitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateBookingCommandHandler(ICommandUnitOfWork command, IQueryUnitOfWork query, ICurrentUserService currentUserService)
        {
            _commandUnitOfWork = command;
            _queryUnitOfWork = query;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<int>> Handle(CreateBookingCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Create a new PaymentMethod entity
                var paymentMethod = new PaymentMethod
                {
                    Name = request.PaymentMethodName,
                    Description = request.PaymentMethodDescription
                };

                // Check if the Id is already set, otherwise generate a new one
                if (paymentMethod.Id == Guid.Empty)
                {
                    paymentMethod.Id = Guid.NewGuid();
                }

                // Create a new Transaction entity
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    Amount = request.PaymentAmount,
                    PaymentMethod = paymentMethod
                };

                // Convert the string UserId to Guid
                Guid userId;
                if (!Guid.TryParse(_currentUserService.UserId, out userId))
                {
                    // Handle invalid UserId format
                    return new ApiResponse<int>
                    {
                        Success = false,
                        StatusCode = HttpStatusCodes.BadRequest,
                        Data = 0,
                        Message = "Invalid UserId format."
                    };
                }

                // Create a new Booking entity
                var entity = new MovieBooking.Domain.Entities.Booking
                {
                    UserId = userId,
                    ShowtimeID = Guid.Parse(request.ShowtimeID),
                    SeatsBooked = request.SeatsBooked
                };

                // Add the booking entity to the repository
                await _commandUnitOfWork.CommandRepository<MovieBooking.Domain.Entities.Booking>().AddAsync(entity);

                // Save changes to get the booking ID
                var saveResult = await _commandUnitOfWork.SaveAsync(cancellationToken);

                // Check if the booking was successfully saved
                if (saveResult <= 0)
                {
                    return new ApiResponse<int>
                    {
                        Success = false,
                        StatusCode = HttpStatusCodes.BadRequest,
                        Data = 0,
                        Message = "Failed to create booking."
                    };
                }

                // Update seat statuses to "Booked"
                //foreach (var seatId in request.SeatID.Split(','))
                //{
                //    var seat = await _queryUnitOfWork.seatQueryRepository.GetByIdAsync(Guid.Parse(seatId));
                //    seat.Status = "Booked";
                //    _commandUnitOfWork.CommandRepository<MovieBooking.Domain.Entities.Seat>().Update(seat);
                //}
                await _commandUnitOfWork.SaveAsync(cancellationToken);

                // Associate the transaction with the booking by setting the BookingId property
                transaction.BookingId = entity.Id;

                // Add the transaction entity to the repository
                await _commandUnitOfWork.CommandRepository<MovieBooking.Domain.Entities.Transaction>().AddAsync(transaction);

                // Save changes
                var transactionSaveResult = await _commandUnitOfWork.SaveAsync(cancellationToken);

                // Check if the transaction was successfully saved
                if (transactionSaveResult <= 0)
                {
                    return new ApiResponse<int>
                    {
                        Success = false,
                        StatusCode = HttpStatusCodes.BadRequest,
                        Data = 0,
                        Message = "Failed to create transaction."
                    };
                }

                // Set the PaymentMethodID to the current PaymentMethod's Id
                transaction.PaymentMethodID = paymentMethod.Id;

                // Update the Transaction entity in the repository
                _commandUnitOfWork.CommandRepository<MovieBooking.Domain.Entities.Transaction>().Update(transaction);

                // Save changes
                var paymentMethodSaveResult = await _commandUnitOfWork.SaveAsync(cancellationToken);

                var response = new ApiResponse<int>
                {
                    Success = paymentMethodSaveResult > 0,
                    StatusCode = paymentMethodSaveResult > 0 ? HttpStatusCodes.Created : HttpStatusCodes.BadRequest,
                    Data = paymentMethodSaveResult,
                    Message = paymentMethodSaveResult > 0 ? "Booking added successfully" : "Failed to create booking."
                };
                return response;
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                return new ApiResponse<int>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.InternalServerError,
                    Data = 0,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

    }
}
