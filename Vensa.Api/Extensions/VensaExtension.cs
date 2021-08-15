//  VensaExtension.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       12:10:47 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Vensa.Api.Entities;
using Vensa.Api.Repositories;
using Vensa.Api.Services;

namespace Vensa.Api.Extensions
{
    public static class VensaExtension
    {
        public static void AddVensaCore(this IServiceCollection services)
        {
            services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.TryAddScoped<IConsumerService, ConsumerService>();
            services.TryAddScoped<IProviderService, ProviderService>();
            services.TryAddScoped<IUnitOfWork, UnitOfWork>();
            services.TryAddScoped<ISQLFactory, SQLFactory>();
        }

        public static void EnhanceQuery(this VensaContext contex)
        {
            try
            {
                //contex.Database.ExecuteSqlRaw()
            }
            catch
            {
                throw;
            }
        }
    }
}
