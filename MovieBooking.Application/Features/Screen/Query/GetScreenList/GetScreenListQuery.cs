using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Screen.Dto;
using MovieBooking.Application.Models.Specification.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Query.GetScreenList
{
    public sealed record GetScreenListQuery : IRequest<IPagedDataResponse<ScreenListDto>>
    {
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }
}
