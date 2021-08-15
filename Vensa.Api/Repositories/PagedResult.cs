//  PagedResult.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       17:13:56 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Configuration.Annotations;

namespace Vensa.Api.Repositories
{
    public class PagedResult<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        [Ignore]
        public T PageData { get; set; }
        
    }
}
