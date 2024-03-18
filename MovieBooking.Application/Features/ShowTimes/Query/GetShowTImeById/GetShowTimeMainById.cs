using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.ShowTimes.Dto;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Query.GetShowTImeById
{
    public record GetShowTimeMainDetailsQueryRequest(Guid id) : IRequest<ApiResponse<ShowTimeMainDetail>>;


    public class GetShowTimeMainDetailsQueryHandler : IRequestHandler<GetShowTimeMainDetailsQueryRequest, ApiResponse<ShowTimeMainDetail>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;
        public GetShowTimeMainDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ShowTimeMainDetail>> Handle(GetShowTimeMainDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var showtime = await _query.QueryRepository<Domain.Entities.ShowtimeMain>().GetByIdAsync(request.id.ToString());

            if (showtime == null)
            {
                Console.WriteLine($"Showtime with ID {request.id} not found.");
                throw new NotFoundException("ShowtimeId", request.id);
            }

            // Proceed with mapping and response creation
            var showTimeDetailDto = _mapper.Map<ShowTimeMainDetail>(showtime);

            var response = new ApiResponse<ShowTimeMainDetail>
            {
                Success = true, // Assuming success if we reach here
                StatusCode = HttpStatusCodes.OK,
                Data = showTimeDetailDto,
                Message = $"Showtime {ConstantMessages.DataFound}"
            };

            return response;
        }

    }
}
