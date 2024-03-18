using MediatR;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Seat.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Query.GetSeatMainList
{
    public class GetAllseatMainQuery : IRequest<List<MovieBooking.Application.Features.Seat.Dto.SeatMainDetailDto>>
    {
    }

    public class GetAllSeatMainQueryHandler : IRequestHandler<GetAllseatMainQuery, List<MovieBooking.Application.Features.Seat.Dto.SeatMainDetailDto>>
    {
        private readonly ISeatQueryRepository _seatQueryRepository;

        public GetAllSeatMainQueryHandler(ISeatQueryRepository seatQueryRepository)
        {
            _seatQueryRepository = seatQueryRepository;
        }

        public async Task<List<MovieBooking.Application.Features.Seat.Dto.SeatMainDetailDto>> Handle(GetAllseatMainQuery request, CancellationToken cancellationToken)
        {
            return await _seatQueryRepository.GetAllSeatMainAsync(cancellationToken);
        }
    }

}