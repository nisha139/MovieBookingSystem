﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Seat.Command.Create;
using MovieBooking.Application.Features.Seat.Command.Delete;
using MovieBooking.Application.Features.Seat.Command.Update;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Features.Seat.Query.GetBookedSeatList;
using MovieBooking.Application.Features.Seat.Query.GetSeatById;
using MovieBooking.Application.Features.Seat.Query.GetSeatList;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Identity.Authorizations.Permissions;
using MovieBooking.Identity.Authorizations;
using Action = MovieBooking.Identity.Authorizations.Action;
using MovieBooking.Application.Features.Movie.Queries;
using MovieBooking.Application.Features.Seat.Query.GetSeatMainList;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Domain.Entities.SeatMain>>> GetAllSeats()
        {
            var query = new GetAllseatMainQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }
        //[Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public async Task<ApiResponse<SeatMainDetailDto>> GetSeatDetails(Guid id)
        {
            return await Mediator.Send(new GetSeatMainDetailsQueryRequest(id));
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost("Create")]
        [MustHavePermission(Action.Create, Resource.Seat)]
        public async Task<ApiResponse<int>> CreateSeat(CreateSeatCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        [MustHavePermission(Action.Update, Resource.Seat)]
        public async Task<ApiResponse<string>> UpdateSeat(Guid id, UpdateSeatCommandRequest request)
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

        //[Authorize(Roles = "Administrator")]
        [HttpPost("Search")]
        [MustHavePermission(Action.Search, Resource.Seat)]
        public async Task<IPagedDataResponse<SeatListDto>> GetSeatListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetSeatListQuery() { PaginationFilter = request });
        }

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        [MustHavePermission(Action.Delete, Resource.Seat)]
        public async Task<ApiResponse<string>> DeleteSeat(Guid id)
        {
            return await Mediator.Send(new DeleteSeatCommandRequest(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("SearchBookedSeat")]
        public async Task<IPagedDataResponse<SeatListDto>> GetBookedSeatListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetBookedSeatListQuery() { PaginationFilter = request });
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("AvailableSeats/{screenId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SeatDetailDto>>>> GetAvailableSeats(Guid screenId)
        {
            try
            {
                var response = await Mediator.Send(new GetAvailableSeatDetailsQueryRequest(screenId));
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
