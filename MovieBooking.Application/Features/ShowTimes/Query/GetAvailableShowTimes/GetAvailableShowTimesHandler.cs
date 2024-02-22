using MediatR;
using MovieBooking.Application.Contracts.Persistence.Repositoris.ShowTime.Query;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.ShowTimes.Dto;
using MovieBooking.Application.Models.Specification.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Query.GetAvailableShowTimes
{
    public class GetAvailableShowTimesForScreenHandler : IRequestHandler<GetAvailableShowTimesQuery, List<ShowTimeDetailDto>>
    {
        private readonly IShowTimeQueryRepostory _showTimeRepository;

        public GetAvailableShowTimesForScreenHandler(IShowTimeQueryRepostory showTimeRepository)
        {
            _showTimeRepository = showTimeRepository;
        }

        public async Task<List<ShowTimeDetailDto>> Handle(GetAvailableShowTimesQuery request, CancellationToken cancellationToken)
        {
            return _showTimeRepository.GetAvailableShowTimes(request.MovieId, request.TheaterId);
        }
    }
}
