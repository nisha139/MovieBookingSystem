using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Controllers.Base;
using MovieBooking.Application.Contracts.Responses;
using MovieBooking.Application.Features.Booking.Command.Create;
using MovieBooking.Application.Features.Booking.Command.Delete;
using MovieBooking.Application.Features.Booking.Command.Update;
using MovieBooking.Application.Features.Booking.Dto;
using MovieBooking.Application.Features.Booking.Query.GetBookingByID;
using MovieBooking.Application.Features.Booking.Query.GetBookingList;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Models.Specification.Filters;

namespace MovieBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseApiController

    {
        [Authorize(Roles = "Administrator,User")]
        [HttpGet("{id}")]
        public async Task<ApiResponse<BookingDetailDto>> GetBookingDetails(Guid id)
        {
            return await Mediator.Send(new GetBookingDetailsQueryRequest(id));
        }

        [Authorize(Roles = "Administrator,User")]
        [HttpPost("Create")]
        //[MustHavePermission(Action.Create, Resource.Movie)]
        public async Task<ApiResponse<int>> CreateBooking(CreateBookingCommandRequest request)
        {
            return await Mediator.Send(request);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<ApiResponse<string>> UpdateBooking(Guid id, UpdateBookingRequestCommand request)
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

        [Authorize(Roles = "Administrator")]
        [HttpPost("Search")]
        public async Task<IPagedDataResponse<BookingListDto>> GetBookingListAsync(PaginationFilter request)
        {
            return await Mediator.Send(new GetBookingListQuery() { PaginationFilter = request });
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> DeleteBooking(Guid id)
        {
            return await Mediator.Send(new DeleteBookingCommandRequest(id));
        }
    }
}
