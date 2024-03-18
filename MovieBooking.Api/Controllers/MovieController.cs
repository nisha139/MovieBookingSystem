using Microsoft.AspNetCore.Http;
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
using MovieBooking.Identity.Authorizations.Permissions;
using MovieBooking.Identity.Authorizations;
using Action = MovieBooking.Identity.Authorizations.Action;
using Microsoft.AspNetCore.Authorization;
using MovieBooking.Application.Features.Movie.Queries;
using MovieBooking.Application.Features.Movie.Query.GetMovieByID;

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
        // [Authorize(Roles ="Administrator")]
        //[HttpPost("Create")]
        //[MustHavePermission(Action.Create, Resource.Movie)]     
        //public async Task<ApiResponse<int>> CreateMovie(CreateMovieCommandRequest request)
        //{
        //    return await Mediator.Send(request);
        //}

        //[HttpGet("{id}")]
        //public async Task<ApiResponse<MovieBooking.Application.Features.Movie.Dto. MovieBooking>> GetMovieDetail(Guid id)
        //{
        //    return await Mediator.Send(new GetMovieBookingDetailsQueryRequest(id));
        //}

        //[Authorize(Roles = "Administrator")]
        //[HttpPut("{id}")]
        //[MustHavePermission(Action.Update, Resource.Movie)]
        //public async Task<ApiResponse<string>> UpdateMovie(Guid id, UpdateMovieRequestCommand request)
        //{
        //    if (id != request.Id)
        //    {
        //        return new ApiResponse<string>
        //        {
        //            Success = false,
        //            Data = "The provided ID in the route does not match the ID in the request body.",
        //            StatusCode = HttpStatusCodes.BadRequest
        //        };
        //    }
        //    return await Mediator.Send(request);
        //}
        [Authorize(Roles = "Administrator")]
        [HttpPost("Create")]
        //[MustHavePermission(Action.Create, Resource.Movie)]
        public async Task<ApiResponse<int>> CreateMovieBooking(CreateMovieBookingCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        //[Authorize(Roles = "Administrator,User")]
        [HttpPost("Search")]
        [MustHavePermission(Action.Search, Resource.Movie)]
        public async Task<IPagedDataResponse<MovieListDto>> GetMovieListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetMovieListQuery() { PaginationFilter = request });
        }

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        [MustHavePermission(Action.Delete, Resource.Movie)]
        public async Task<ApiResponse<string>> DeleteMovie(Guid id)
        {
            return await Mediator.Send(new DeleteMovieCommandRequest(id));
        }

        [HttpGet]
        public async Task<ActionResult<List<Domain.Entities.MovieBooking>>> GetAllMovies()
        {
            var query = new GetAllMoviesQuery();
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        //[MustHavePermission(Action.Update, Resource.Movie)]
        public async Task<ApiResponse<string>> UpdateMovies(Guid id, UpdateMovieBookingCommandRequest request)
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

    }
}
