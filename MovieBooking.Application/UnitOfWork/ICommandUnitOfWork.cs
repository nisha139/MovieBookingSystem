using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Domain.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieBooking.Application.UnitOfWork
{
    public interface ICommandUnitOfWork : IDisposable
    {
        ICommandRepository<TEntity> CommandRepository<TEntity>() where TEntity : BaseEntity, new();

        Task<int> SaveAsync(CancellationToken cancellationToken);
        Task BeginTransaction();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
