using MediatR;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Update
{
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingRequestCommand, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitOfWork;
        private readonly IQueryUnitOfWork queryUnitOfWork;

        public UpdateBookingCommandHandler(ICommandUnitOfWork commandUnitOfWork,IQueryUnitOfWork queryUnitOfWork)
        {
            _commandUnitOfWork = commandUnitOfWork;
            queryUnitOfWork = queryUnitOfWork;
        }

        public async Task<ApiResponse<string>> Handle(UpdateBookingRequestCommand request, CancellationToken cancellationToken)
        {
            var existingBooking = await _commandUnitOfWork.CommandRepository<Domain.Entities.Booking>().GetByIdAsync(request.Id, cancellationToken);

            if (existingBooking == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = HttpStatusCodes.NotFound,
                    Message = $"Booking with ID {request.Id} not found.",
                };
            }

            // Update booking details
            existingBooking.ShowtimeID = request.ShowtimeID;
            existingBooking.UserId = request.UserId;
            existingBooking.SeatsBooked = request.SeatsBooked;

            // Update associated transaction if it exists
            var existingTransaction = await queryUnitOfWork.QueryRepository<Domain.Entities.Transaction>().GetAsync(t => t.BookingId == request.Id);
            if (existingTransaction != null)
            {
                // Update transaction details
                existingTransaction.PaymentMethodID = request.PaymentMethodId;
                existingTransaction.Amount = request.TransactionAmount;
                // Assuming other transaction details need to be updated as well
                // existingTransaction.OtherProperty = request.OtherProperty;
            }

            // Update associated payment method if transaction exists and payment method is found
            if (existingTransaction != null && existingTransaction.PaymentMethodID != null)
            {
                var existingPaymentMethod = await queryUnitOfWork.QueryRepository<PaymentMethod>().GetAsync(p => p.Id == existingTransaction.PaymentMethodID);
                if (existingPaymentMethod != null)
                {
                    // Update payment method details
                    existingPaymentMethod.Name = request.PaymentMethodName;
                    existingPaymentMethod.Description = request.PaymentMethodDescription;
                    // Add more attributes as needed
                }
            }

            // Save changes
            await _commandUnitOfWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = "Booking updated successfully",
                Message = $"Booking {ConstantMessages.UpdatedSuccessfully}",
            };

            return response;
        }

    }
}
