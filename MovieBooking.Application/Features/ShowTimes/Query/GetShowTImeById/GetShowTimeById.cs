using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.ShowTimes.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.Features.Theater.Query.GetTheaterByID;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.ShowTimes.Query.GetShowTImeById
{
    public record GetShowTimeDetailsQueryRequest(Guid id) : IRequest<ApiResponse<ShowTimeDetailDto>>;

    public class GetShowTimeDetailsQueryHandler : IRequestHandler<GetShowTimeDetailsQueryRequest, ApiResponse<ShowTimeDetailDto>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;
        public GetShowTimeDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ShowTimeDetailDto>> Handle(GetShowTimeDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var theater = await _query.QueryRepository<Domain.Entities.Showtime>().GetByIdAsync(request.id.ToString());
            _ = theater ?? throw new NotFoundException("ShowTimeId ", request.id);

            var theaterDetailDto = _mapper.Map<ShowTimeDetailDto>(theater);

            var response = new ApiResponse<ShowTimeDetailDto>
            {
                Success = theaterDetailDto != null,
                StatusCode = theaterDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = theaterDetailDto!,
                Message = theaterDetailDto != null ? $"ShowTime {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} showtime."
            };

            return response;
        }
    }
}
