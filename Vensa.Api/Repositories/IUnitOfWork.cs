//  IUnitOfWork.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       11:49:43 13/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Vensa.Api.Repositories
{
    public interface IUnitOfWork
    {
        IEnumerable<T> ExecuteSqlQuery<T>(string query, params SqlParameter[] paramters);
    }
}
