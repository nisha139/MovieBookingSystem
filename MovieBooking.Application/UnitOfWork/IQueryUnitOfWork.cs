using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Booking.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Movie.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Screen.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Seat.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.ShowTime.Query;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Theater.Query;
using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.UnitOfWork
{
    public interface IQueryUnitOfWork
    {
        //Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class;
        IQueryRepository<TEntity> QueryRepository<TEntity>() where TEntity : BaseEntity, new();

        IMovieQueryRepository movieQueryRepository { get; }
        ITheaterQueryRepository theaterQueryRepository { get; }
        IScreenQueryRepository ScreenQueryRepository { get; }
        ISeatQueryRepository seatQueryRepository { get; }
        IShowTimeQueryRepostory showTimeQueryRepostory { get; }
        IBookingQueryRepository bookingQueryRepository { get; }
    }
}
