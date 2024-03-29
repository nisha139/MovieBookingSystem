﻿using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Domain.Common;
using MovieBooking.Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.Repositories.Base
{
    public class QueryRepository<T> : IQueryRepository<T> where T : BaseEntity, new()
    {
        protected readonly MovieDBContext context;

        public QueryRepository(MovieDBContext context)
        {
            this.context = context;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await context.Set<T>().Where(predicate).AnyAsync();
            return result;
        }

        public async Task<IQueryable<T>> GetAllAsync(bool isChangeTracking = false)
        {
            IQueryable<T> query = context.Set<T>();
            if (isChangeTracking)
            {
                return query = query.AsNoTracking().AsQueryable();

                //This method is called after AsNoTracking() to convert the result into an IQueryable<T>.
                //This enables further LINQ querying capabilities on the collection.
            }
            return await Task.Run(() => query.AsQueryable());


        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public async Task<IEnumerable<T>> GetAllWithIncludeAsync(bool isChangeTracking = false, Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            IQueryable<T> query = context.Set<T>();

            if (isChangeTracking)
            {
                query = predicate is null ? query.AsNoTracking()
                                          : query.Where(predicate).AsNoTracking();
                // It's typically used when you only need to read data from the database and you don't intend
                // to make any changes to the entities or save them back to the database. By calling AsNoTracking()
                if (includes != null)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }
            }
            else
            {
                query = predicate is null ? query
                                          : query.Where(predicate);
                if (includes != null)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }
            }

            return await query.ToListAsync();

        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool isChangeTracking = false)
        {
            IQueryable<T> query = context.Set<T>();
            if (isChangeTracking)
            {
                query = query.Where(predicate).AsNoTracking();
            }
            else
            {
                query = query.Where(predicate);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return await query.FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<T> GetByIdAsync(string id, bool isChangeTracking = false)
        {
            IQueryable<T> query = context.Set<T>();
            if (isChangeTracking)
            {
                query = query.Where(e => e.Id == Guid.Parse(id)).AsNoTracking();
            }
            else
            {
                query = query.Where(e => e.Id == Guid.Parse(id));
            }

#pragma warning disable CS8603 // Possible null reference return.
            return await query.SingleOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<T> GetWithIncludeAsync(bool isChangeTracking, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            if (isChangeTracking)
            {
                query = query.Where(predicate);
                if (includes is not null)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }
            }
            else
            {
                query = query.Where(predicate).AsNoTracking();
                if (includes is not null)
                {
                    foreach (var item in includes)
                    {
                        query = query.Include(item);
                    }
                }
            }

#pragma warning disable CS8603 // Possible null reference return.
            return await query.SingleOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.

        }
    }
}
