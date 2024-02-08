using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Movie.Query.GetTaskByID;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Query.GetTheaterByID
{
    public record GetTheaterDetailsQueryRequest(Guid id) : IRequest<ApiResponse<TheaterDetailDto>>;

    public class GetTheaterDetailsQueryHandler : IRequestHandler<GetTheaterDetailsQueryRequest, ApiResponse<TheaterDetailDto>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;
        public GetTheaterDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<TheaterDetailDto>> Handle(GetTheaterDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var theater = await _query.QueryRepository<Domain.Entities.Theater>().GetByIdAsync(request.id.ToString());
            _ = theater ?? throw new NotFoundException("MovieId ", request.id);

            var theaterDetailDto = _mapper.Map<TheaterDetailDto>(theater);

            var response = new ApiResponse<TheaterDetailDto>
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
