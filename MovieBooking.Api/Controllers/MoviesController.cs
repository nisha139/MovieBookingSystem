//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using MovieBooking.Api.Controllers.Base;
//using MovieBooking.Application.Exceptions;
//using MovieBooking.Application.Features.Common;
//using MovieBooking.Application.Features.Movie.Query.GetMovieByID;

//namespace MovieBooking.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MoviesController : BaseApiController
//    {
//        [HttpGet("{id}")]
//        public async Task<ActionResult<ApiResponse<MovieBooking.Application.Features.Movie.Dto.MovieBooking>>> GetMoviesDetail(Guid id)
//        {
//            try
//            {
//                var response = await Mediator.Send(new GetMovieBookingDetailsQueryRequest(id));
//                return Ok(response);
//            }
//            catch (NotFoundException ex)
//            {
//                return NotFound(new ApiResponse<MovieBooking.Application.Features.Movie.Dto.MovieBooking>
//                {
//                    Success = false,
//                    Message = ex.Message
//                });
//            }
//            catch (Exception ex)
//            {
//                // Log the exception
//                // Return a generic error response
//                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<MovieBooking.Application.Features.Movie.Dto.MovieBooking>
//                {
//                    Success = false,
//                    Message = "An error occurred while fetching movie details. Please try again later."
//                });
//            }
//        }
//    }
//}
