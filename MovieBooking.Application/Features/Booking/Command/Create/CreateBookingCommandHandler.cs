using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieBooking.Application.Contracts.Application;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Entities;

namespace MovieBooking.Application.Features.Booking.Command.Create
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommandRequest, ApiResponse<int>>
    {
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _seatLocks = new ConcurrentDictionary<string, SemaphoreSlim>();
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
            List<string> seatIdList = null;

            try
            {
                seatIdList = request.SeatID.Split(',').ToList();
                // Lock seats to prevent concurrent bookings with a timeout
                if (!await LockSeats(seatIdList, TimeSpan.FromSeconds(30)))
                {
                    return new ApiResponse<int>
                    {
                        Success = false,
                        StatusCode = HttpStatusCodes.BadRequest,
                        Data = 0,
                        Message = "Failed to acquire locks for seats. Please try again later."
                    };
                }

                // Begin transaction
                await _commandUnitOfWork.BeginTransaction();

                // Get the seat entities for the requested seats
                var seatsToBook = new List<Domain.Entities.Seat>();
                foreach (var seatId in seatIdList)
                {
                    var seat = await _queryUnitOfWork.seatQueryRepository.GetByIdAsync(Guid.Parse(seatId));
                    seatsToBook.Add(seat);
                }

                // Check if any of the seats are already booked
                if (seatsToBook.Any(seat => seat.Status == "Booked"))
                {
                    // Rollback transaction
                    await _commandUnitOfWork.RollbackTransactionAsync();

                    return new ApiResponse<int>
                    {
                        Success = false,
                        StatusCode = HttpStatusCodes.Conflict, // HTTP status code 409 Conflict
                        Data = 0,
                        Message = "One or more seats are already booked."
                    };
                }

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

                // Update seat statuses to "Booked"
                foreach (var seat in seatsToBook)
                {
                    seat.Status = "Booked";
                    _commandUnitOfWork.CommandRepository<MovieBooking.Domain.Entities.Seat>().Update(seat);
                }

                // Associate the transaction with the booking by setting the BookingId property
                transaction.BookingId = entity.Id;

                // Add the transaction entity to the repository
                await _commandUnitOfWork.CommandRepository<MovieBooking.Domain.Entities.Transaction>().AddAsync(transaction);

                // Commit transaction
                await _commandUnitOfWork.CommitTransactionAsync();

                // Save changes to get the booking ID
                await _commandUnitOfWork.SaveAsync(cancellationToken);

                return new ApiResponse<int>
                {
                    Success = true,
                    StatusCode = HttpStatusCodes.Created,
                    Data = entity.Id.GetHashCode(),
                    Message = "Booking added successfully"
                };
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of exception
                await _commandUnitOfWork.RollbackTransactionAsync();

                // Handle exception appropriately
                return new ApiResponse<int>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.InternalServerError,
                    Data = 0,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
            finally
            {
                // Release locks
                if (seatIdList != null)
                {
                    await ReleaseSeats(seatIdList);
                }
            }
        }

        private async Task<bool> LockSeats(IEnumerable<string> seatIds, TimeSpan timeout)
        {
            var tasks = seatIds.Select(seatId => LockSeatWithTimeout(seatId, timeout));
            var completedTask = await Task.WhenAny(tasks);
            return completedTask.Result;
        }

        private async Task<bool> LockSeatWithTimeout(string seatId, TimeSpan timeout)
        {
            var semaphore = _seatLocks.GetOrAdd(seatId, _ => new SemaphoreSlim(1));
            return await semaphore.WaitAsync(timeout);
        }

        private async Task ReleaseSeats(IEnumerable<string> seatIds)
        {
            foreach (var seatId in seatIds)
            {
                // Release the lock for each seat
                if (_seatLocks.ContainsKey(seatId))
                {
                    _seatLocks[seatId].Release();
                }
            }
        }
    }
}
