//using AutoMapper;
//using MediatR;
//using MovieBooking.Application.Exceptions;
//using MovieBooking.Application.Features.Screen.Dto;
//using MovieBooking.Application.UnitOfWork;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace MovieBooking.Application.Features.Screen.Query
//{
//    public record GetScreensWithBookedSeatsQuery : IRequest<List<ScreenListDto>>;

//    public class GetScreensWithBookedSeatsQueryHandler : IRequestHandler<GetScreensWithBookedSeatsQuery, List<ScreenListDto>>
//    {
//        private readonly IQueryUnitOfWork _query;
//        private readonly IMapper _mapper;

//        public GetScreensWithBookedSeatsQueryHandler(IQueryUnitOfWork query, IMapper mapper)  
//        {
//            _query = query;
//            _mapper = mapper;
//        }

//        public async Task<List<ScreenListDto>> Handle(GetScreensWithBookedSeatsQuery request, CancellationToken cancellationToken)
//        {
//            // Query screens with booked seats
//            var screensWithBookedSeats = await _query.QueryRepository<Domain.Entities.Screen>()
//                .Where(s => s.Seats.Any(seat => seat.Status == "Booked"))
//                .ToListAsync();

//            // Map entities to DTOs
//            var screenListDtos = _mapper.Map<List<ScreenListDto>>(screensWithBookedSeats);

//            return screenListDtos;
//        }
//    }
//}
