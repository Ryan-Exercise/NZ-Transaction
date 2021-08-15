//  IConsumerService.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       10:28:19 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Vensa.Api.Dtos;
using Vensa.Api.Repositories;

namespace Vensa.Api.Services
{
    public interface IConsumerService
    {
        ConsumerDto GetConsumerWithBalance(string firstName, string lastName, string dateOfBirth);
        PagedResult<IEnumerable<TransactionDto>> GetConsumerTransactions(long consumerId, int pageNumber, int pageSize);
    }
}
