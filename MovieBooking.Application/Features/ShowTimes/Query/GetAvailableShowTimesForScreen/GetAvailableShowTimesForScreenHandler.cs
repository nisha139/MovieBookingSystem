using MediatR;
using MovieBooking.Application.Contracts.Persistence.Repositoris.ShowTime.Query;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.ShowTimes.Dto;
using MovieBooking.Application.Features.ShowTimes.Query.GetAvailableShowTimesForScreen;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Query.GetAvailableShowTimesForScreen
{
    public class GetAvailableShowTimesForScreenHandler : IRequestHandler<GetAvailableShowTimesForScreenQuery, List<ShowTimeDetailDto>>
    {
        private readonly IShowTimeQueryRepostory _showTimeRepository;

        public GetAvailableShowTimesForScreenHandler(IShowTimeQueryRepostory showTimeRepository)
        {
            _showTimeRepository = showTimeRepository;
        }

        public async Task<List<ShowTimeDetailDto>> Handle(GetAvailableShowTimesForScreenQuery request, CancellationToken cancellationToken)
        {
            return _showTimeRepository.GetAvailableShowTimesForScreen(request.MovieId, request.TheaterId, request.ScreenID);
        }
    }
}
