using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Persistence.Repositoris.Base
{
    public interface ICommandRepository<T> where T : BaseEntity, new()
    {
        Task<T> AddAsync(T entity);
        System.Threading.Tasks.Task AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }


}
