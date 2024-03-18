using MediatR;
using System;
using System.Collections.Generic;
using MovieBooking.Application.Features.ShowTimes.Dto;

namespace MovieBooking.Application.Features.ShowTimes.Query.GetAllShowTime
{
    public class GetAllShowTimeMainQuery : IRequest<List<ShowTimeMainDetail>>
    {
        public Guid MovieId { get; set; }
        public Guid TheaterId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
