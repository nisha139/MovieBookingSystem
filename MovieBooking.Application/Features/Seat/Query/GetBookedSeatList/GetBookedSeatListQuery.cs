using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Models.Specification.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Seat.Query.GetBookedSeatList
{
    public class GetBookedSeatListQuery : IRequest<IPagedDataResponse<SeatListDto>>
    {
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }
}
