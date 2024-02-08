using MovieBooking.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Application.Contracts.Persistence.Repositoris.Base
{
    public interface IQueryRepository<T> where T  : BaseEntity,new()
    {
        Task<IQueryable<T>> GetAllAsync(bool isChangeTracking = false);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool isChangeTracking = false);
        Task<T> GetByIdAsync(string id, bool isChangeTracking = false);
    }
}
