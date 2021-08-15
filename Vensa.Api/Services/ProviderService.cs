//  ProviderService.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       23:25:8 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Vensa.Api.Dtos;
using Vensa.Api.Entities;
using Vensa.Api.Repositories;
using Vensa.Api.Extensions;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;

namespace Vensa.Api.Services
{
    public class ProviderService : IProviderService
    {

        #region Private properties

        private readonly IRepository<Consumer> _consumerRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<TransactionMethod> _methodRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISQLFactory _sqlFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<ConsumerService> _logger;
        // Field from statistics data
        private static readonly ISet<string> STAT_FIELDS = new HashSet<string>
        {
            "lasttransactiontime",
            "mostfrequentmethod",
            "balance"
        };

        #endregion

        public ProviderService(IRepository<Consumer> consumerRepository,
            IRepository<Transaction> transactionRepository,
            IRepository<TransactionMethod> methodRepository,
            IUnitOfWork unitOfWork,
            ISQLFactory sqlFactory,
            IMapper mapper,
            ILogger<ConsumerService> logger)
        {
            _consumerRepository = consumerRepository;
            _transactionRepository = transactionRepository;
            _methodRepository = methodRepository;
            _unitOfWork = unitOfWork;
            _sqlFactory = sqlFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public PagedResult<IEnumerable<ListConsumerDto>> GetConsumerList(
            int pageNumber = 1, int pageSize = 100,
            string orderBy = "Id", string orderDirection = "asc")
        {
            var result = new PagedResult<IEnumerable<ListConsumerDto>>();
            int page;
            var parameters = GenerateParameters(out page, pageNumber, pageSize, orderBy, orderDirection);
            var consumers = _unitOfWork.ExecuteSqlQuery<ListConsumerDto>(
                GenerateSQL(orderBy), parameters.ToArray());
            var totalItems = _consumerRepository.GetAll().Count();
            result.TotalItems = totalItems;
            result.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            result.PageSize = pageSize;
            result.PageNumber = page;
            result.PageData = consumers;

            return result;
        }
        /// <summary>
        /// OrderBy and Search for Balance, LastTransactionTime and MostFrequentMethod not supported
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="field">Balance, LastTransactionTime and MostFrequentMethod not supported</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy">Balance, LastTransactionTime and MostFrequentMethod not supported</param>
        /// <param name="orderDirection">asc(default) | desc</param>
        /// <returns></returns>
        public PagedResult<IEnumerable<ListConsumerDto>> GetConsumerList(
            string keyword, string field, int pageNumber = 1, int pageSize = 100,
            string orderBy = "Id", string orderDirection = "asc")
        {
            PagedResult<IEnumerable<ListConsumerDto>> result = null;
            SqlParameter searchParam = new($"@{field}", GetFinalKeyword(keyword, field));
            
            string cond = searchParam.ToConditions();
            if (!string.IsNullOrEmpty(cond)) // Field found
            {
                int page;
                var parameters = GenerateParameters(out page, pageNumber, pageSize, orderBy, orderDirection);
                parameters.Add(searchParam);
                var consumers = _unitOfWork.ExecuteSqlQuery<ListConsumerDto>(
                    GenerateSQL(orderBy, field, cond),
                    parameters.ToArray());
                result = new PagedResult<IEnumerable<ListConsumerDto>>();
                var totalItems = _consumerRepository.GetAll().Count();
                result.TotalItems = totalItems;
                result.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                result.PageSize = pageSize;
                result.PageNumber = page;
                result.PageData = consumers;

                return result;
            }

            return result ?? new PagedResult<IEnumerable<ListConsumerDto>>();
        }

        private string GenerateSQL(
            string orderBy, string field="", string cond = "")
        {
            if (string.IsNullOrEmpty(field)) // ListConsumers
            {
                return STAT_FIELDS.Contains(orderBy) ?
                    "mostfrequentmethod".Equals(orderBy) ?
                    _sqlFactory.CreateForMostFrequentMethodField(cond)
                    : _sqlFactory.CreateForStatField(cond)
                    : _sqlFactory.CreateForCommonField(cond);
            }
            else // SearchConsumers
            {
                if (STAT_FIELDS.Contains(field) || STAT_FIELDS.Contains(orderBy))
                {
                    throw new NotSupportedException($"{field} seach not supported now");
                }
                else
                {
                    return _sqlFactory.CreateForCommonField(cond);
                }
            }
        }

        private List<SqlParameter> GenerateParameters( out int page,
            int pageNumber, int pageSize, string orderBy, string orderDirection)
        {
            page = Math.Max(pageNumber, 1);
            int offset = (page - 1) * pageSize;
            return new List<SqlParameter>
            {
                new SqlParameter("@OrderBy", orderBy),
                new SqlParameter("@OrderDirection", orderDirection.Equals("desc")),
                new SqlParameter("@Offset", offset),
                new SqlParameter("@PageSize", pageSize)
            };
        }

        private object GetFinalKeyword(string keyword, string field)
        {
            if ("mostfrequentmethod".Equals(field))
            {
                var r = _methodRepository.GetAll(
                    m => m.TransactionMethodName.ToLower().Equals(keyword));
                return r.FirstOrDefault()?.Id;

            }
            return keyword;
        }
    }
}
