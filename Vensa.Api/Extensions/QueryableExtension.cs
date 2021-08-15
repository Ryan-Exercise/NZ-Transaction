//  DataPagerExtension.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       17:10:50 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vensa.Api.Repositories;

namespace Vensa.Api.Extensions
{
    public static class QueryableExtension
    {
        public static PagedResult<IQueryable<T>> Paginate<T>(
            this IQueryable<T> query,
            int page,
            int limit)
            where T : class
        {
            page = Math.Max(1, page);
            var start = (page - 1) * limit;
            var data = query.Skip(start).Take(limit);

            var result = new PagedResult<IQueryable<T>>
            {
                PageNumber = page,
                PageSize = limit,
                TotalItems = query.Count()
            };
            result.TotalPages = (int)Math.Ceiling(result.TotalItems / (double)limit);
            result.PageData = data;

            return result;
        }

        public static IOrderedQueryable OrderBy<T>(this IQueryable<T> query, string orderPropertype)
        {
            var entityType = typeof(T);
            var propertyInfo = entityType.GetProperty(orderPropertype);
            ParameterExpression arg = Expression.Parameter(entityType, "o");
            MemberExpression property = Expression.Property(arg, orderPropertype);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        public static IOrderedQueryable OrderByDescending<T>(this IQueryable<T> query, string orderPropertype)
        {
            var entityType = typeof(T);

            var propertyInfo = entityType.GetProperty(orderPropertype);
            ParameterExpression arg = Expression.Parameter(entityType, "o");
            MemberExpression property = Expression.Property(arg, orderPropertype);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable)genericMethod.Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

    }
}
