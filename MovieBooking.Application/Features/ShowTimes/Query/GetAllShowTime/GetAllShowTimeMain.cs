using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MovieBooking.Application.Contracts.Persistence.Repositoris.ShowTime.Query;
using MovieBooking.Application.Features.ShowTimes.Dto;

namespace MovieBooking.Application.Features.ShowTimes.Query.GetAllShowTime
{
    public class GetAllShowtimeMainQueryHandler : IRequestHandler<GetAllShowTimeMainQuery, List<ShowTimeMainDetail>>
    {
        private readonly IShowTimeQueryRepostory _showTimeQueryRepository;

        public GetAllShowtimeMainQueryHandler(IShowTimeQueryRepostory showTimeQueryRepository)
        {
            _showTimeQueryRepository = showTimeQueryRepository;
        }

        public async Task<List<ShowTimeMainDetail>> Handle(GetAllShowTimeMainQuery request, CancellationToken cancellationToken)
        {
            // Implement the logic to handle the query
            return await _showTimeQueryRepository.GetAllShowtimeMainAsync(cancellationToken);
        }
    }
}
