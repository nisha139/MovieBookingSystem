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
using MovieBooking.Persistence.Repositories.ShowTIme.Query;
using MovieBooking.Persistence.Repositories.Theater.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.UnitofWork
{
    public class QueryUnitofWork : IQueryUnitOfWork
    {
        private readonly MovieDBContext _appDbContext;
        private Hashtable _repositories;
        private readonly ISeatQueryRepository _seatQueryRepository;
        public QueryUnitofWork(MovieDBContext appDbContext, ISeatQueryRepository seatQueryRepository)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _appDbContext = appDbContext;
            _seatQueryRepository = seatQueryRepository;
        }
        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await _appDbContext.SaveChangesAsync(cancellationToken);
        }

        public IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(QueryRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _appDbContext);

                _repositories.Add(type, repositoryInstance);
            }
            // Ensure _repositories[type] is not null before returning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return (IQueryRepository<TEntity>)_repositories[type] ?? new QueryRepository<TEntity>(_appDbContext);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        public IMovieQueryRepository _movieQueryRepository
        {
            get
            {
                if (_repositories == null) _repositories = new Hashtable();

                var type = typeof(IMovieQueryRepository).Name;

                if (!_repositories.ContainsKey(type))
                {
                    var repositoryType = typeof(IMovieQueryRepository);
                    var repositoryInstance = Activator.CreateInstance(repositoryType, _appDbContext);

                    _repositories.Add(type, repositoryInstance);
                }
                // Ensure _repositories[type] is not null before returning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                return (IMovieQueryRepository)_repositories[type] ?? new MovieQueryRepository(_appDbContext);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
        }
        public ISeatQueryRepository seatQueryRepository => _seatQueryRepository;
        public ITheaterQueryRepository theaterQueryRepository
        {
            get
            {
                if (_repositories == null) _repositories = new Hashtable();

                var type = typeof(IMovieQueryRepository).Name;

                if (!_repositories.ContainsKey(type))
                {
                    var repositoryType = typeof(IMovieQueryRepository);
                    var repositoryInstance = Activator.CreateInstance(repositoryType, _appDbContext);

                    _repositories.Add(type, repositoryInstance);
                }
                // Ensure _repositories[type] is not null before returning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                return (ITheaterQueryRepository)_repositories[type] ?? new TheaterQueryRepository(_appDbContext);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
        }
        public BookingQueryRepository _bookingQueryRepository;
        public ShowTimeRepository _ShowtimeQueryRepository;
        public ScreenQueryRepository _ScreenQueryRepository;
        public TheaterQueryRepository _theaterQueryRepository;

        public ITheaterQueryRepository TheaterQueryRepository => _theaterQueryRepository ?? new TheaterQueryRepository(_appDbContext);
        public IScreenQueryRepository ScreenQueryRepository => _ScreenQueryRepository ?? new ScreenQueryRepository(_appDbContext);
        //public IMovieQueryRepository movieQueryRepository ;
        //public ISeatQueryRepository seatQueryRepository => throw new NotImplementedException();
        public IShowTimeQueryRepostory showTimeQueryRepostory => _ShowtimeQueryRepository ?? new ShowTimeRepository(_appDbContext);
        public IBookingQueryRepository bookingQueryRepository => _bookingQueryRepository ?? new BookingQueryRepository(_appDbContext);

        public ISeatQueryRepository seatQueryRepositorys => new SeatQueryRepository(_appDbContext);
        public IMovieQueryRepository movieQueryRepository => _movieQueryRepository ?? new MovieQueryRepository(_appDbContext);
        

        //public ISeatQueryRepository seatQueryRepository => throw new NotImplementedException();

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
