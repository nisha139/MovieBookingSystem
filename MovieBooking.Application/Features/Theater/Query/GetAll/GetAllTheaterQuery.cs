using MediatR;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Theater.Query;
using MovieBooking.Application.Features.Movie.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.Features.Theater.Queries.GetAll
{
    public class GetAllTheaterQuery : IRequest<List<MovieBooking.Application.Features.Theater.Dto.TheaterMainDto>>
    {
    }

    public class GetAllTheaterQueryHandler : IRequestHandler<GetAllTheaterQuery, List<MovieBooking.Application.Features.Theater.Dto.TheaterMainDto>>
    {
        private readonly ITheaterQueryRepository _theaterQueryRepository;

        public GetAllTheaterQueryHandler(ITheaterQueryRepository theaterQueryRepository)
        {
            _theaterQueryRepository = theaterQueryRepository;
        }

        public async Task<List<MovieBooking.Application.Features.Theater.Dto.TheaterMainDto>> Handle(GetAllTheaterQuery request, CancellationToken cancellationToken)
        {
            return await _theaterQueryRepository.GetAllTheaterAsync(cancellationToken);
        }
    }
}
