using AutoMapper;
using MediatR;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Features.Common;
using MovieBooking.Application.Features.Movie.Dto;
using MovieBooking.Application.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Query.GetTaskByID
{
    public record GetMovieDetailsQueryRequest(Guid id) : IRequest<ApiResponse<MovieDetailDto>>;

    public class GetMovieDetailsQueryHandler : IRequestHandler<GetMovieDetailsQueryRequest, ApiResponse<MovieDetailDto>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;

        public GetMovieDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<MovieDetailDto>> Handle(GetMovieDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var movie = await _query.QueryRepository<Domain.Entities.Movie>().GetByIdAsync(request.id.ToString());
            _ = movie ?? throw new NotFoundException("MovieId ", request.id);

            var movieDetailDto = _mapper.Map<MovieDetailDto>(movie);

            var response = new ApiResponse<MovieDetailDto>
            {
                Success = movieDetailDto != null,
                StatusCode = movieDetailDto != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = movieDetailDto!,
                Message = movieDetailDto != null ? $"Movie {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} Movie."
            };

            return response;
        }
    }
}
