using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.Features.Movie.Query.GetTaskByID;
using MovieBooking.Application.Features.Screen.Dto;
using MovieBooking.Application.Features.Theater.Dto;
using MovieBooking.Application.Features.Theater.Query.GetTheaterByID;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Screen.Query.GetScreenById
{
    public record GetScreenDetailsQueryRequest(Guid id) : IRequest<ApiResponse<ScreenTheaterDetail>>;

    public class GetScreenDetailsQueryHandler : IRequestHandler<GetScreenDetailsQueryRequest, ApiResponse<ScreenTheaterDetail>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;
        public GetScreenDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }
        public async Task<ApiResponse<ScreenTheaterDetail>> Handle(GetScreenDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var screen = await _query.QueryRepository<Domain.Entities.Screen>().GetByIdAsync(request.id.ToString());
            _ = screen ?? throw new NotFoundException("ScreenId ", request.id);

            // Fetch theater information based on TheaterId
            var theater = await _query.QueryRepository<Domain.Entities.Theater>().GetByIdAsync(screen.TheaterId.ToString());
            _ = theater ?? throw new NotFoundException("TheaterId", screen.TheaterId);

            var ScreenDetailDto = _mapper.Map<ScreenTheaterDetail>(screen);
            ScreenDetailDto.TheaterName = theater.Name; // Include TheaterName in ScreenDetailDto

            var response = new ApiResponse<ScreenTheaterDetail>
            {
                Success = ScreenDetailDto != null,
                StatusCode = ScreenDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = ScreenDetailDto!,
                Message = ScreenDetailDto != null ? $"Screen {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} screen."
            };

            return response;
        }
    }
}
