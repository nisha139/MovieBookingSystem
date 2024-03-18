using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.ShowTimes.Command.Create;
using MovieBooking.Application.Features.ShowTimes.Command.Delete;
using MovieBooking.Application.Features.ShowTimes.Command.Update;
using MovieBooking.Application.Features.ShowTimes.Dto;
using MovieBooking.Application.Features.ShowTimes.Query.GetAvailableShowTimes;
using MovieBooking.Application.Features.ShowTimes.Query.GetAvailableShowTimesForScreen;
using MovieBooking.Application.Features.ShowTimes.Query.GetShowTImeById;
using MovieBooking.Application.Features.ShowTimes.Query.GetShowTimeList;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Identity.Authorizations.Permissions;
using MovieBooking.Identity.Authorizations;
using Action = MovieBooking.Identity.Authorizations.Action;
using MovieBooking.Application.Features.Seat.Query.GetSeatMainList;
using MovieBooking.Application.Features.ShowTimes.Query.GetAllShowTime;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimeController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Domain.Entities.ShowtimeMain>>> GetAllShowtimes()
        
        {
            var query = new GetAllShowTimeMainQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }
        //[Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public async Task<ApiResponse<ShowTimeMainDetail>> GetShowTimeDetails(Guid id)
        {
            return await Mediator.Send(new GetShowTimeMainDetailsQueryRequest(id));
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost("Create")]
        [MustHavePermission(Action.Create, Resource.Showtime)]
        public async Task<ApiResponse<int>> CreateShowTime(CreateShowTimeCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        [MustHavePermission(Action.Update, Resource.Showtime)]
        public async Task<ApiResponse<string>> UpdateShowTime(Guid id, UpdateShowTimeCommandRequest request)
        {
            if (id != request.ShowTimeId)
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
        public async Task<IPagedDataResponse<ShowTimeListDto>> GetShowTimeListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetShowTimeListQuery() { PaginationFilter = request });
        }

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        [MustHavePermission(Action.Delete, Resource.Showtime)]
        public async Task<ApiResponse<string>> DeleteShowTime(Guid id)
        {
            return await Mediator.Send(new DeleteShowTimeCommandRequest(id));
        }


        //[Authorize(Roles = "Administrator")]
        [HttpGet("AvailableShowTimes/{movieId}/{theaterId}")]
        public async Task<ActionResult<List<ShowTimeDetailDto>>> GetAvailableShowTimes(Guid movieId, Guid theaterId)
        {
            var response = await Mediator.Send(new GetAvailableShowTimesQuery { MovieId = movieId, TheaterId = theaterId });
            return Ok(response);
        }


        //[Authorize(Roles = "Administrator")]
        [HttpGet("AvailableShowTimes/{movieId}/{theaterId}/{ScreenId}")]
        public async Task<ActionResult<List<ShowTimeDetailDto>>> GetAvailableShowTimesForScreens(Guid movieId, Guid theaterId,Guid ScreenId)
        {
            var response = await Mediator.Send(new GetAvailableShowTimesForScreenQuery { MovieId = movieId, TheaterId = theaterId , ScreenID=ScreenId });
            return Ok(response);
        }
    }
}
