﻿using MovieBooking.Application.Contracts.Persistence.Repositoris.Base;
using MovieBooking.Application.UnitOfWork;
using MovieBooking.Domain.Common;
using MovieBooking.Persistence.Database;
using MovieBooking.Persistence.Repositories.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBooking.Persistence.UnitofWork
{
    public class CommandUnitOfWork : ICommandUnitOfWork
    {
        private readonly MovieDBContext _dBContext;
        private Hashtable _repositories;
        public CommandUnitOfWork(MovieDBContext dBContext) 
        {
            _dBContext = dBContext;
        }

        public ICommandRepository<TEntity> CommandRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(CommandRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dBContext);

                _repositories.Add(type, repositoryInstance);
            }
            // Ensure _repositories[type] is not null before returning
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return (ICommandRepository<TEntity>)_repositories[type] ?? new CommandRepository<TEntity>(_dBContext);
        }

        public void Dispose()
        {
            _dBContext.Dispose();
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            var result = await _dBContext.SaveChangesAsync(cancellationToken);
            return result;
        }

    }
}
