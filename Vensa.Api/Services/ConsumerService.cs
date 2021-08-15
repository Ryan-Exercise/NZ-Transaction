//  ConsumerService.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       11:53:20 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Vensa.Api.Dtos;
using Vensa.Api.Entities;
using Vensa.Api.Exceptions;
using Vensa.Api.Extensions;
using Vensa.Api.Repositories;

namespace Vensa.Api.Services
{
    public class ConsumerService : IConsumerService
    {
        #region Private properties

        private readonly IRepository<Consumer> _consumerRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<TransactionType> _typeRepository;
        private readonly IRepository<TransactionMethod> _methodRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ConsumerService> _logger;

        #endregion

        public ConsumerService(
            IRepository<Consumer> consumerRepository,
            IRepository<Transaction> transactionRepository,
            IRepository<TransactionType> typeRepository,
            IRepository<TransactionMethod> methodRepository,
            IMapper mapper,
            ILogger<ConsumerService> logger)
        {
            _consumerRepository = consumerRepository;
            _transactionRepository = transactionRepository;
            _typeRepository = typeRepository;
            _methodRepository = methodRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public ConsumerDto GetConsumerWithBalance(string firstName, string lastName, string dateOfBirth)
        {
            Expression<Func<Consumer, bool>> firstNameExp =
                c => c.FirstName.Equals(firstName)
                    || c.MiddleName.Equals(firstName)
                    || c.PreferredName.Equals(firstName);
            var consumers = _consumerRepository.GetAll(firstNameExp
                .And(c => c.LastName.Equals(lastName))
                .And(c => c.DateOfBirth.Equals(DateTime.Parse(dateOfBirth))));
            if (consumers.Any())
            {
                int count = consumers.AsEnumerable().Count();
                if(count == 1)
                {
                    var consumer = consumers.FirstOrDefault();
                    var consumerDto = _mapper.Map<Consumer, ConsumerDto>(consumer);
                    var transactions = _transactionRepository
                        .GetAll(t => t.ConsumerId == consumer.Id)
                        .GroupBy(t => t.ConsumerId)
                        .Select(t => new
                        {
                            LastTransactionDate = t.Max(a => a.DateTime),
                            Balance = t.Sum(a => a.Value)
                        });
                    var stats = transactions.FirstOrDefault();
                    consumerDto.Balance = stats.Balance;
                    consumerDto.LastTransactionTime = stats.LastTransactionDate;
                    return consumerDto;
                }
                else
                {
                    throw new MultipleEntitiesFoundException($"Consumer({firstName} {lastName})");
                }
            }
            else
            {
                throw new EntityNotFoundException($"Consumer({firstName} {lastName})");
            }
        }

        public PagedResult<IEnumerable<TransactionDto>> GetConsumerTransactions(long consumerId, int pageNumber, int pageSize)
        {
            var consumer = _consumerRepository.GetById(consumerId);
            if(consumer == null)
            {
                throw new EntityNotFoundException($"Consumer(#{consumerId}) not found");
            }
            var pagedT = _transactionRepository.GetAll(pageNumber, pageSize,
                c => c.ConsumerId == consumerId);
            var types = _typeRepository.GetAll();
            var methods = _methodRepository.GetAll();

            var orderedT = pagedT.PageData
                .OrderByDescending(t => t.DateTime)
                .Join(types,
                tran => tran.TransactionType,
                type => type.Id,
                (tran, type) =>
                new {
                    tran.Id,
                    tran.DateTime,
                    tran.Value,
                    TransactionType = type.TransactionTypeName,
                    tran.TransactionMethod
                })
                .Join(methods,
                tran => tran.TransactionMethod,
                method => method.Id,
                (tran, method) =>
                new TransactionDto
                {
                    Id = tran.Id,
                    DateTime = tran.DateTime,
                    Value = tran.Value,
                    TransactionType = tran.TransactionType,
                    TransactionMethod = method.TransactionMethodName
                }
                ).AsEnumerable();
            
            if(orderedT.Any())
            {
                var result = _mapper.Map<PagedResult<IQueryable<Transaction>>,
                PagedResult<IEnumerable<TransactionDto>>>(pagedT);
                result.PageData = orderedT;
                return result;
            }
            else
            {
                var empty = new PagedResult<IEnumerable<TransactionDto>>();
                empty.PageData = new List<TransactionDto>();
                return empty;
            }
        }
    }

}
