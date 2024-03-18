using Microsoft.AspNetCore.Authorization;
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
using MovieBooking.Application.Features.Theater.Query.GetTheaterScreensByTitle;
using MovieBooking.Application.Models.Specification.Filters;
using MovieBooking.Identity.Authorizations.Permissions;
using MovieBooking.Identity.Authorizations;
using Action = MovieBooking.Identity.Authorizations.Action;
using MovieBooking.Application.Features.Theater.Command.CreateTheater;
using MovieBooking.Application.Features.Movie.Queries;
using MovieBooking.Application.Features.Theater.Queries.GetAll;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Domain.Entities.TheaterMain>>> GetAllTheaters()
        {
            var query = new GetAllTheaterQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        //[Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public async Task<ApiResponse<TheaterMainDetailDto>> GetTheaterDetails(Guid id)
        {
            return await Mediator.Send(new GetTheaterMainDetailsQueryRequest(id));
        }

        //[Authorize(Roles = "Administrator")]
        //[HttpPost("Create")]
        //[MustHavePermission(Action.Create, Resource.Theater)]
        //public async Task<ApiResponse<int>> CreateTheater(CreateTheaterCommandRequest request)
        //{
        //    return await Mediator.Send(request);
        //}

        //[Authorize(Roles = "Administrator")]
        [HttpPost("Search")]
        [MustHavePermission(Action.Search, Resource.Theater)]
        public async Task<IPagedDataResponse<TheaterListDto>> GetTheaterListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetTheaterListQuery() { PaginationFilter = request });
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        [MustHavePermission(Action.Update, Resource.Theater)]
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

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        [MustHavePermission(Action.Delete, Resource.Theater)]
        public async Task<ApiResponse<string>> DeleteTheater(Guid id)
        {
            return await Mediator.Send(new DeleteTheaterCommandRequest(id));
        }
        [HttpPost("Create")]
        [MustHavePermission(Action.Create, Resource.Theater)]
        public async Task<ApiResponse<int>> CreateTheaters(CreateTheaterMainCommandRequest request)
        {
            return await Mediator.Send(request);
        }
        [Authorize(Roles = "Administrator,User")]
        [HttpGet("GetByMovieTitle/{title}")]
        public async Task<ActionResult<ApiResponse<List<TheaterDto>>>> GetTheatersByMovieTitle(string title)
        {
            return  await Mediator.Send(new GetTheatersByMovieTitleQueryRequest(title));
        }

        [Authorize(Roles = "Administrator,User")]
        [HttpGet("details/{theaterId}")]
        public async Task<IActionResult> GetTheaterDetail(Guid theaterId)
        {
            var query = new GetTheaterWithScreensQueryRequest(theaterId);
            var theaterResponse = await Mediator.Send(query);

            if (theaterResponse.Success)
            {
                return Ok(theaterResponse);
            }
            else
            {
                return StatusCode((int)theaterResponse.StatusCode, theaterResponse);
            }
        }

    }
}
