    using AutoMapper;
    using MediatR;
    using MovieBooking.Application.Exceptions;
    using MovieBooking.Application.Features.Common;
    using MovieBooking.Application.Features.Movie.Dto;
    using MovieBooking.Application.Features.Movie.Query.GetTaskByID;
    using MovieBooking.Application.UnitOfWork;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Movie.Query.GetMovieByID
{
    public record GetMovieBookingDetailsQueryRequest(Guid id) : IRequest<ApiResponse<MovieBooking.Application.Features.Movie.Dto.MovieBooking>>;

    public class GetMovieBookingDetailsQueryHandler : IRequestHandler<GetMovieBookingDetailsQueryRequest, ApiResponse<MovieBooking.Application.Features.Movie.Dto.MovieBooking>>
    {
        private readonly IQueryUnitOfWork _query;
        private readonly IMapper _mapper;

        public GetMovieBookingDetailsQueryHandler(IQueryUnitOfWork query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<ApiResponse<MovieBooking.Application.Features.Movie.Dto.MovieBooking>> Handle(GetMovieBookingDetailsQueryRequest request, CancellationToken cancellationToken)
        {
            var movie = await _query.QueryRepository<Domain.Entities.MovieBooking>().GetByIdAsync(request.id.ToString());
            _ = movie ?? throw new NotFoundException("MovieId ", request.id);

            var movie1 = _mapper.Map<MovieBooking.Application.Features.Movie.Dto.MovieBooking>(movie);

            var response = new ApiResponse<MovieBooking.Application.Features.Movie.Dto.MovieBooking>
            {
                Success = movie1 != null,
                StatusCode = movie1 != null ? HttpStatusCodes.OK : HttpStatusCodes.BadRequest,
                Data = movie1!,
                Message = movie1 != null ? $"Movie {ConstantMessages.DataFound}" : $"{ConstantMessages.NotFound} Movie."
            };

            return response;
        }
    }
}


