using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.Models.Specification.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Query.GetSeatList
{
    public sealed record GetSeatListQuery : IRequest<IPagedDataResponse<SeatListDto>>
    {
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }
}
