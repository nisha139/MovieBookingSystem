using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Models.Specification.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Booking.Query.GetBookingList
{
    public sealed record GetBookingListQuery : IRequest<IPagedDataResponse<BookingListDto>>
    {
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }
}