using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Screen.Command.Create;
using MovieBooking.Application.Features.Screen.Command.Delete;
using MovieBooking.Application.Features.Screen.Command.Update;
using MovieBooking.Application.Features.Screen.Dto;
using MovieBooking.Application.Features.Screen.Query.GetScreenById;
using MovieBooking.Application.Features.Screen.Query.GetScreenList;
using MovieBooking.Application.Models.Specification.Filters;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<ApiResponse<ScreenTheaterDetail>> GetScreenDetails(Guid id)
        {
            return await Mediator.Send(new GetScreenDetailsQueryRequest(id));
        }

        [HttpPost("Create")]
        public async Task<ApiResponse<int>> CreateScreen(CreateScreenCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<string>> UpdateScreen(Guid id, UpdateScreenRequestCommand request)
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
        public async Task<IPagedDataResponse<ScreenListDto>> GetScreenListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetScreenListQuery() { PaginationFilter = request });
        }

        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteScreen(Guid id)
        {
            return await Mediator.Send(new DeleteScreenCommandRequest(id));
        }
    }
}
