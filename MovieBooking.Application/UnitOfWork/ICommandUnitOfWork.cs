using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.UnitOfWork
{
    public interface ICommandUnitOfWork : IDisposable
    {
        ICommandRepository<TEntity> CommandRepository<TEntity>() where TEntity : BaseEntity, new();

        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
