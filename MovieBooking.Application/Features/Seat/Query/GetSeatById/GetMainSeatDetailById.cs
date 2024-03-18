using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.UnitOfWork;

namespace MovieBooking.Application.Features.Seat.Query.GetSeatById
{
    public record GetSeatMainDetailsQueryRequest(Guid id) : IRequest<ApiResponse<SeatMainDetailDto>>;

    public class seatSeatMainDetailsQueryHandler : IRequestHandler<GetSeatMainDetailsQueryRequest, ApiResponse<SeatMainDetailDto>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;

        public seatSeatMainDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<SeatMainDetailDto>> Handle(GetSeatMainDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var theater = await _query.QueryRepository<Domain.Entities.SeatMain>().GetByIdAsync(request.id.ToString());
            _ = theater ?? throw new NotFoundException("SeatId ", request.id);

            SeatMainDetailDto? theaterDetailDto = null;
            if (theater != null)
            {
                theaterDetailDto = _mapper.Map<SeatMainDetailDto>(theater);
            }

            var response = new ApiResponse<SeatMainDetailDto>
            {
                Success = theaterDetailDto != null,
                StatusCode = theaterDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = theaterDetailDto,
                Message = theaterDetailDto != null ? $"Seat {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} seat."
            };

            return response;
        }
    }
}
