﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Command.Create;
using MovieBooking.Application.Features.Movie.Command.Delete;
using MovieBooking.Application.Features.Movie.Command.Update;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Movie.Query.GetTaskByID;
using MovieBooking.Application.Features.Tasks.Query;
using MovieBooking.Application.Models.Specification.Filters;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<ApiResponse<MovieDetailDto>> GetMovieDetails(Guid id)
        {
            return await Mediator.Send(new GetMovieDetailsQueryRequest(id));
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<int>> CreateMovie(CreateMovieCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<string>> UpdateMovie(Guid id, UpdateMovieRequestCommand request)
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

        [HttpPost("Search")]
        public async Task<IPagedDataResponse<MovieListDto>> GetMovieListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetMovieListQuery() { PaginationFilter = request });
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteMovie(Guid id)
        {
            return await Mediator.Send(new DeleteMovieCommandRequest(id));
        }
    }
}