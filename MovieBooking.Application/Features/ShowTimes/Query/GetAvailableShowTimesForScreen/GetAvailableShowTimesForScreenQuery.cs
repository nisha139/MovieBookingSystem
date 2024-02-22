using MediatR;
using MovieBooking.Application.Features.ShowTimes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Query.GetAvailableShowTimesForScreen
{
    public record GetAvailableShowTimesForScreenQuery : IRequest<List<ShowTimeDetailDto>>
    {
        public Guid MovieId { get; init; }
        public Guid TheaterId { get; init; }
        public Guid ScreenID { get; init; }
    }
}
