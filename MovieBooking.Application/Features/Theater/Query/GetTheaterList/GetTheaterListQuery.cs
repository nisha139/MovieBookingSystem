using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.Models.Specification.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Query.GetTheaterList
{
    public sealed record GetTheaterListQuery : IRequest<IPagedDataResponse<TheaterListDto>>
    {
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }

}
