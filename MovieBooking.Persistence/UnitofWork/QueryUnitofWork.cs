using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Booking.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Screen.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Seat.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.ShowTime.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Theater.Query;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Common;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Repositories.Base;
using MovieBooking.Persistence.Repositories.Booking.Query;
using MovieBooking.Persistence.Repositories.Movie.Query;
using MovieBooking.Persistence.Repositories.Screen.Query;
using MovieBooking.Persistence.Repositories.Seat.Query;
using MovieBooking.Persistence.Repositories.Theater.Query;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.UnitOfWork
{
    public class QueryUnitOfWork : IQueryUnitOfWork
    {
        private readonly MovieDBContext _appDbContext;
        private readonly ISeatQueryRepository _seatQueryRepository;
        private readonly IMovieQueryRepository _movieQueryRepository;
        private readonly ITheaterQueryRepository _theaterQueryRepository;
        private readonly IScreenQueryRepository _screenQueryRepository;
        private readonly IShowTimeQueryRepostory _showTimeQueryRepostory;
        private readonly IBookingQueryRepository _bookingQueryRepository;

        public QueryUnitOfWork(
            MovieDBContext appDbContext,
            ISeatQueryRepository seatQueryRepository,
            IMovieQueryRepository movieQueryRepository,
            ITheaterQueryRepository theaterQueryRepository,
            IScreenQueryRepository screenQueryRepository,
            IShowTimeQueryRepostory showTimeQueryRepostory,
            IBookingQueryRepository bookingQueryRepository)
        {
            _appDbContext = appDbContext;
            _seatQueryRepository = seatQueryRepository;
            _movieQueryRepository = movieQueryRepository;
            _theaterQueryRepository = theaterQueryRepository;
            _screenQueryRepository = screenQueryRepository;
            _showTimeQueryRepostory = showTimeQueryRepostory;
            _bookingQueryRepository = bookingQueryRepository;
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            return new QueryRepository<TEntity>(_appDbContext);
        }

        public IMovieQueryRepository movieQueryRepository => _movieQueryRepository;
        public ITheaterQueryRepository theaterQueryRepository => _theaterQueryRepository;
        public IScreenQueryRepository ScreenQueryRepository => _screenQueryRepository;
        public ISeatQueryRepository seatQueryRepository => _seatQueryRepository;
        public IShowTimeQueryRepostory showTimeQueryRepostory => _showTimeQueryRepostory;
        public IBookingQueryRepository bookingQueryRepository => _bookingQueryRepository;

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
