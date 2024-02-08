﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Command.Create;
using MovieBooking.Application.Features.Theater.Command.Delete;
using MovieBooking.Application.Features.Theater.Command.Update;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.Features.Theater.Query.GetTheaterByID;
using MovieBooking.Application.Features.Theater.Query.GetTheaterList;
using MovieBooking.Application.Features.Theater.Query.GetTheatersByMovieTitleQuery;
using MovieBooking.Application.Models.Specification.Filters;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<ApiResponse<TheaterDetailDto>> GetTheaterDetails(Guid id)
        {
            return await Mediator.Send(new GetTheaterDetailsQueryRequest(id));
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<int>> CreateTheater(CreateTheaterCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost("Search")]
        public async Task<IPagedDataResponse<TheaterListDto>> GetTheaterListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetTheaterListQuery() { PaginationFilter = request });
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<string>> UpdateTheater(Guid id, UpdateTheaterRequestCommand request)
        {
            if (id != request.Id)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Data = "The provided ID in the route does not match the ID in the request body.",
                    StatusCode = HttpStatusCodes.BadRequest
                };
            }
            return await Mediator.Send(request);
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteTheater(Guid id)
        {
            return await Mediator.Send(new DeleteTheaterCommandRequest(id));
        }

        [HttpGet("GetByMovieTitle/{title}")]
        public async Task<ActionResult<ApiResponse<List<TheaterDto>>>> GetTheatersByMovieTitle(string title)
        {
            return  await Mediator.Send(new GetTheatersByMovieTitleQueryRequest(title));
        }
    }
}