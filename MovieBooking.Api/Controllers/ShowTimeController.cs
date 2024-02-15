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
using MovieBooking.Application.Features.ShowTimes.Query.GetShowTImeById;
using MovieBooking.Application.Features.ShowTimes.Query.GetShowTimeList;
using MovieBooking.Application.Models.Specification.Filters;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimeController : BaseApiController
    {
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public async Task<ApiResponse<ShowTimeDetailDto>> GetShowTimeDetails(Guid id)
        {
            return await Mediator.Send(new GetShowTimeDetailsQueryRequest(id));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("Create")]
        public async Task<ApiResponse<int>> CreateShowTime(CreateShowTimeCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
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

        [Authorize(Roles = "Administrator")]
        [HttpPost("Search")]
        public async Task<IPagedDataResponse<ShowTimeListDto>> GetShowTimeListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetShowTimeListQuery() { PaginationFilter = request });
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteShowTime(Guid id)
        {
            return await Mediator.Send(new DeleteShowTimeCommandRequest(id));
        }
    }
}
