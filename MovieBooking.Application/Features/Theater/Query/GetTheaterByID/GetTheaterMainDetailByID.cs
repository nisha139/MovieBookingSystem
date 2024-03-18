using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Query.GetTheaterByID
{
    public record GetTheaterMainDetailsQueryRequest(Guid id) : IRequest<ApiResponse<TheaterMainDetailDto>>;

    public class GetTheaterMainDetailsQueryHandler : IRequestHandler<GetTheaterMainDetailsQueryRequest, ApiResponse<TheaterMainDetailDto>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;
        public GetTheaterMainDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<TheaterMainDetailDto>> Handle(GetTheaterMainDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var theater = await _query.QueryRepository<Domain.Entities.TheaterMain>().GetByIdAsync(request.id.ToString());
            _ = theater ?? throw new NotFoundException("MovieId ", request.id);

            var theaterDetailDto = _mapper.Map<TheaterMainDetailDto>(theater);

            var response = new ApiResponse<TheaterMainDetailDto>
            {
                Success = theaterDetailDto != null,
                StatusCode = theaterDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = theaterDetailDto!,
                Message = theaterDetailDto != null ? $"Movie {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} Movie."
            };

            return response;
        }
    }
}
