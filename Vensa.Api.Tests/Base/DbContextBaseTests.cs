//  DbContextBaseTests.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       12:25:14 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vensa.Api.Entities;

namespace Vensa.Api.Tests.Base
{
    public class DbContextBaseTests : IDisposable
    {
        private const string _connectionString =
            "Server=localhost,1433;User Id=sa;Password=34U8&4ie(b;Database=Vensa;";
        private readonly DbConnection _connection;

        protected DbContextOptions<VensaContext> Options { get; }
        public DbContextBaseTests()
        {
            Options = new DbContextOptionsBuilder<VensaContext>().UseSqlServer(_connectionString).Options;
        }

        public void Dispose() => _connection?.Dispose();
    }
}
