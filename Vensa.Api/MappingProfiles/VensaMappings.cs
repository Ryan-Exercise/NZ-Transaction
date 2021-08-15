//  VensaMappings.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       14:28:3 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Vensa.Api.Dtos;
using Vensa.Api.Entities;
using Vensa.Api.Repositories;

namespace Vensa.Api.MappingProfiles
{
    public class VensaMappings : Profile
    {
        public VensaMappings()
        {
            CreateMap<Consumer, ConsumerDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<PagedResult<IQueryable<Transaction>>,
                PagedResult<IEnumerable<TransactionDto>>>().ReverseMap();
            CreateMap<Consumer, ListConsumerDto>().ReverseMap();
            CreateMap<PagedResult<IQueryable<Consumer>>,
                PagedResult<IEnumerable<ListConsumerDto>>>().ReverseMap();
        }
    }
}
