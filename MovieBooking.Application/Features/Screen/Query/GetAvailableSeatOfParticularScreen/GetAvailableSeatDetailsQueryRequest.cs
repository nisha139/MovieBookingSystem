using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.UnitOfWork;

namespace MovieBooking.Application.Features.Seat.Query.GetBookedSeatList
{
    public record GetAvailableSeatDetailsQueryRequest(Guid ScreenId) : IRequest<ApiResponse<IEnumerable<SeatDetailDto>>>;

    public class GetAvailableSeatDetailsQueryHandler : IRequestHandler<GetAvailableSeatDetailsQueryRequest, ApiResponse<IEnumerable<SeatDetailDto>>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;
        public GetAvailableSeatDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<SeatDetailDto>>> Handle(GetAvailableSeatDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var seats = await _query.QueryRepository<Domain.Entities.Seat>().GetAllAsync(); 
            if (seats == null || !seats.Any())
                throw new NotFoundException("Seats", request.ScreenId);

            var availableSeats = seats.Where(seat => seat.ScreenId == request.ScreenId && seat.Status != "Booked").ToList();

            var seatDetailDtos = _mapper.Map<IEnumerable<SeatDetailDto>>(availableSeats);

            var response = new ApiResponse<IEnumerable<SeatDetailDto>>
            {
                Success = seatDetailDtos.Any(),
                StatusCode = seatDetailDtos.Any() ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = seatDetailDtos.Any() ? seatDetailDtos : null,
                Message = seatDetailDtos.Any() ? $"{ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} seats."
            };

            return response;
        }
    }
}
