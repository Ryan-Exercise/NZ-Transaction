//  Repository.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       11:29:47 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vensa.Api.Entities;
using Vensa.Api.Exceptions;
using Vensa.Api.Extensions;

namespace Vensa.Api.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        #region Private properties

        private VensaContext _context;
        private ILogger<Repository<T>> _logger;
        private bool _disposed;
        #endregion

        public Repository(VensaContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public IQueryable<T> GetAll(
            Expression<Func<T, bool>> conds,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (conds != null)
            {
                return _context.Set<T>().Where(conds);
            }
            return _context.Set<T>();
        }

        public PagedResult<IQueryable<T>> GetAll(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> conds,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            IQueryable<T> query = _context.Set<T>();
            if(conds != null)
            {
                query = query.Where(conds);
            }
            return query.AsNoTracking().Paginate(pageNumber, pageSize);

           
        }

        public T GetById(long id, CancellationToken cancellationToken = default)
        {
            return _context.Set<T>().FirstOrDefault(a => a.Id == id);
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
