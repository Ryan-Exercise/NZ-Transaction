//  IProviderService.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       23:24:50 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using Vensa.Api.Dtos;
using Vensa.Api.Repositories;

namespace Vensa.Api.Services
{
    public interface IProviderService
    {
        PagedResult<IEnumerable<ListConsumerDto>> GetConsumerList(
            int pageNumber = 1, int pageSize = 100,
            string orderBy = "Id", string orderSort = "asc");

        PagedResult<IEnumerable<ListConsumerDto>> GetConsumerList(
            string keyword,string field, int pageNumber = 1, int pageSize = 100,
            string orderBy = "Id", string orderSort = "asc");
    }
}
