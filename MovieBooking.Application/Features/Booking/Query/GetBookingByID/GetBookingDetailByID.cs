using System;
using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Query.GetBookingByID
{
    public record GetBookingDetailsQueryRequest(Guid Id) : IRequest<ApiResponse<BookingDetailDto>>;

    public class GetBookingDetailsQueryHandler : IRequestHandler<GetBookingDetailsQueryRequest, ApiResponse<BookingDetailDto>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;

        public GetBookingDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<BookingDetailDto>> Handle(GetBookingDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            // Get the booking entity by ID
            var booking = await _query.QueryRepository<Domain.Entities.Booking>().GetAsync(b => b.Id == request.Id);

            if (booking == null)
                throw new NotFoundException("Booking", request.Id);

            // Retrieve associated PaymentMethod and Transaction data
            var transaction = await _query.QueryRepository<Transaction>().GetAsync(t => t.BookingId == booking.Id);
            var paymentMethod = await _query.QueryRepository<PaymentMethod>().GetAsync(p => p.Id == transaction.PaymentMethodID);

            // Retrieve the Showtime associated with the booking
            var showtime = await _query.QueryRepository<Domain.Entities.Showtime>().GetAsync(s => s.Id == booking.ShowtimeID);

            // Retrieve the movie associated with the Showtime
            var movie = await _query.QueryRepository<Domain.Entities.Movie>().GetAsync(m => m.Id == showtime.MovieId);

            // Retrieve seat information
            var seat = await _query.QueryRepository<Domain.Entities.Seat>()
    .GetAsync(s => s.Bookings.Any(b => b.Id == request.Id));

            // Retrieve screen information from the seat
            var screen = await _query.QueryRepository<Domain.Entities.Screen>().GetAsync(sc => sc.Id == seat.ScreenId);

            // Map entities to DTO
            var bookingDetailDto = _mapper.Map<BookingDetailDto>(booking);
            bookingDetailDto.PaymentMethod = _mapper.Map<PaymentMethodDto>(paymentMethod);
            bookingDetailDto.Transaction = _mapper.Map<TransactionDto>(transaction);

            // Include the movie name in the DTO
            bookingDetailDto.MovieName = movie.Title;

            // Map the Showtime entity to a Showtime DTO
            var showtimeDto = _mapper.Map<ShowtimeDto>(showtime);
            bookingDetailDto.Showtime = showtimeDto;

            // Map the Seat entity to a Seat DTO
            var seatDto = _mapper.Map<SeatDetailDto>(seat);
            bookingDetailDto.Seat = seatDto;
            // Map the Screen entity to a Screen DTO
            var screenDto = _mapper.Map<ScreenDto>(screen);
            bookingDetailDto.Screen = screenDto;


            var response = new ApiResponse<BookingDetailDto>
            {
                Success = true,
                StatusCode = HttpStatusCodes.OK,
                Data = bookingDetailDto,
                Message = $"{ConstantMessages.DataFound} Booking."
            };

            return response;
        }
    }
}
