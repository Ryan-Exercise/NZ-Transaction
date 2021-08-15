//  IRepository.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       10:17:50 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Vensa.Api.Entities;

namespace Vensa.Api.Repositories
{
    public interface IRepository<T> : IDisposable where T : EntityBase
    {
        T GetById(long id, CancellationToken cancellationToken = default);
        IQueryable<T> GetAll(
            Expression<Func<T, bool>> conds = null, CancellationToken cancellationToken = default);
        PagedResult<IQueryable<T>> GetAll(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> conds = null,
            CancellationToken cancellationToken = default);
    }
}
