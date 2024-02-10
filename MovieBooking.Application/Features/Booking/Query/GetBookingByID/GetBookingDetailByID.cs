using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Query.GetBookingByID
{
    public record GetBookingDetailsQueryRequest(Guid id) : IRequest<ApiResponse<BookingDetailDto>>;
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
            var booking = await _query.QueryRepository<Domain.Entities.Booking>().GetByIdAsync(request.id.ToString());
            _ = booking ?? throw new NotFoundException("MovieId ", request.id);

            var bookingDetailDto = _mapper.Map<BookingDetailDto>(booking);

            var response = new ApiResponse<BookingDetailDto>
            {
                Success = bookingDetailDto != null,
                StatusCode = bookingDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = bookingDetailDto!,
                Message = bookingDetailDto != null ? $"Booking {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} booking."
            };

            return response;
        }
    }
}
