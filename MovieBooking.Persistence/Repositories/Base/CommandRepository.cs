using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Domain.Common;
using MovieBooking.Persistence.Database;
using System;

namespace MovieBooking.Persistence.Repositories.Base
{
    public class CommandRepository<T>(MovieDBContext context) : ICommandRepository<T> where T : BaseEntity, new()
    {

        private readonly MovieDBContext _appDbContext = context;

        public async Task<T> AddAsync(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _appDbContext.Set<T>().AddRangeAsync(entities);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _appDbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public void Remove(T entity)
        {
            _appDbContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _appDbContext.Set<T>().RemoveRange(entities);
        }

        public T Update(T entity)
        {
            _appDbContext.Set<T>().Update(entity);
            return entity;
        }

    }
}


