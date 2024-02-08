using MediatR;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.ShowTimes.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.Models.Specification.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Query.GetShowTimeList
{
    public sealed record GetShowTimeListQuery : IRequest<IPagedDataResponse<ShowTimeListDto>>
    {
        public PaginationFilter PaginationFilter { get; set; } = default!;
    }

}
