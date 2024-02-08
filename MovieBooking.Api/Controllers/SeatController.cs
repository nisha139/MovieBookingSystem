using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Seat.Command.Create;
using MovieBooking.Application.Features.Seat.Command.Delete;
using MovieBooking.Application.Features.Seat.Command.Update;
using MovieBooking.Application.Features.Seat.Dto;
using MovieBooking.Application.Features.Seat.Query.GetSeatById;
using MovieBooking.Application.Features.Seat.Query.GetSeatList;
using MovieBooking.Application.Models.Specification.Filters;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<ApiResponse<SeatDetailDto>> GetSeatDetails(Guid id)
        {
            return await Mediator.Send(new GetSeatDetailsQueryRequest(id));
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<int>> CreateSeat(CreateSeatCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id}")]
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

        [HttpPost("Search")]
        public async Task<IPagedDataResponse<SeatListDto>> GetSeatListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetSeatListQuery() { PaginationFilter = request });
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteSeat(Guid id)
        {
            return await Mediator.Send(new DeleteSeatCommandRequest(id));
        }
    }
}
