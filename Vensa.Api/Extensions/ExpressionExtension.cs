//  ExpressionExtension.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       13:6:53 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Linq.Expressions;

namespace Vensa.Api.Extensions
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> one)
        {
            var candidateExp = one.Parameters[0];
            var body = Expression.Not(one.Body);
            return Expression.Lambda<Func<T, bool>>(body, candidateExp);
        }

        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            var candidateExp = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExp);
            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);
            var body = Expression.And(left, right);
            return Expression.Lambda<Func<T, bool>>(body, candidateExp);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            var candidateExp = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExp);

            var left = parameterReplacer.Replace(one.Body);
            var right = parameterReplacer.Replace(another.Body);
            var body = Expression.Or(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExp);
        }
    }

    public class ParameterReplacer : ExpressionVisitor
    {
        public ParameterExpression ParameterExpression { get; private set; }
        public ParameterReplacer(ParameterExpression paramExp)
        {
            this.ParameterExpression = paramExp;
        }

        public Expression Replace(Expression exp)
        {
            return this.Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this.ParameterExpression;
        }
    }
}
