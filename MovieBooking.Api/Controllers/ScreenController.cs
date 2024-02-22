using Microsoft.AspNetCore.Authorization;
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
using MovieBooking.Identity.Authorizations.Permissions;
using MovieBooking.Identity.Authorizations;
using Action = MovieBooking.Identity.Authorizations.Action;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : BaseApiController
    {
       //[Authorize(Roles = "Administrator,User")]
        [HttpGet("{id}")]
        public async Task<ApiResponse<ScreenTheaterDetail>> GetScreenDetails(Guid id)
        {
            return await Mediator.Send(new GetScreenDetailsQueryRequest(id));
        }
        //[Authorize(Roles = "Administrator")]
        [HttpPost("Create")]
        [MustHavePermission(Action.Create, Resource.Screen)]
        public async Task<ApiResponse<int>> CreateScreen(CreateScreenCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        [MustHavePermission(Action.Update, Resource.Screen)]
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

        //[Authorize(Roles = "Administrator")]
        [HttpPost("Search")]
        [MustHavePermission(Action.Search, Resource.Screen)]
        public async Task<IPagedDataResponse<ScreenListDto>> GetScreenListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetScreenListQuery() { PaginationFilter = request });
        }

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        [MustHavePermission(Action.Delete, Resource.Screen)]
        public async Task<ApiResponse<string>> DeleteScreen(Guid id)
        {
            return await Mediator.Send(new DeleteScreenCommandRequest(id));
        }
    }
}
