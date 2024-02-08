using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Query.GetSeatById
{
    public record GetSeatDetailsQueryRequest(Guid id) : IRequest<ApiResponse<SeatDetailDto>>;

    public class GetSeatDetailsQueryHandler : IRequestHandler<GetSeatDetailsQueryRequest, ApiResponse<SeatDetailDto>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;
        public GetSeatDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<SeatDetailDto>> Handle(GetSeatDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var theater = await _query.QueryRepository<Domain.Entities.Seat>().GetByIdAsync(request.id.ToString());
            _ = theater ?? throw new NotFoundException("SeatId ", request.id);

            var theaterDetailDto = _mapper.Map<SeatDetailDto>(theater);

            var response = new ApiResponse<SeatDetailDto>
            {
                Success = theaterDetailDto != null,
                StatusCode = theaterDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = theaterDetailDto!,
                Message = theaterDetailDto != null ? $"Seat {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} seat."
            };

            return response;
        }
    }
}
