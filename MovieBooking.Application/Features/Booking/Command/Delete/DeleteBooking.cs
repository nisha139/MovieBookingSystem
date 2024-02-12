using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Command.Delete
{
    public record DeleteBookingCommandRequest(Guid Id) : IRequest<ApiResponse<string>>;

    public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommandRequest, ApiResponse<string>>
    {
        private readonly ICommandUnitOfWork _commandUnitOfWork;
        private readonly IQueryUnitOfWork _queryUnitOfWork;

        public DeleteBookingCommandHandler(IQueryUnitOfWork queryUnitOfWork, ICommandUnitOfWork commandUnitOfWork)
        {
            _queryUnitOfWork = queryUnitOfWork;
            _commandUnitOfWork = commandUnitOfWork;
        }

        public async Task<ApiResponse<string>> Handle(DeleteBookingCommandRequest request, CancellationToken cancellationToken)
        {
            // Retrieve the booking
            var booking = await _queryUnitOfWork.QueryRepository<Domain.Entities.Booking>().GetByIdAsync(request.Id.ToString());
            if (booking == null)
            {
                throw new NotFoundException("Booking", request.Id);
            }

            // Retrieve the associated transaction
            var transaction = await _queryUnitOfWork.QueryRepository<Domain.Entities.Transaction>().GetAsync(t => t.BookingId == request.Id);
            if (transaction != null)
            {
                // Delete the transaction
                _commandUnitOfWork.CommandRepository<Domain.Entities.Transaction>().Remove(transaction);
            }

            // Retrieve the associated payment method
            if (transaction != null && transaction.PaymentMethodID != null)
            {
                var paymentMethod = await _queryUnitOfWork.QueryRepository<Domain.Entities.PaymentMethod>().GetAsync(p => p.Id == transaction.PaymentMethodID);
                if (paymentMethod != null)
                {
                    // Delete the payment method
                    _commandUnitOfWork.CommandRepository<Domain.Entities.PaymentMethod>().Remove(paymentMethod);
                }
            }

            // Delete the booking
            _commandUnitOfWork.CommandRepository<Domain.Entities.Booking>().Remove(booking);

            // Save changes
            var result = await _commandUnitOfWork.SaveAsync(cancellationToken);

            var response = new ApiResponse<string>
            {
                Success = result > 0,
                StatusCode = result > 0 ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = result.ToString(),
                Message = result > 0 ? $"{ConstantMessages.DeletedSuccessfully} Booking" : $"{ConstantMessages.FailedToDelete} Booking"
            };

            return response;
        }
    }
}
