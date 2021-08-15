//  UnitOfWork.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       11:51:18 13/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Vensa.Api.Entities;

namespace Vensa.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VensaContext _context;

        private readonly static IDictionary<string, IDictionary<string,PropertyInfo>>
            PROPERTY_CACHE = new Dictionary<string, IDictionary<string, PropertyInfo>>();

        public UnitOfWork(VensaContext context)
        {
            _context = context;
           
        }

        public IEnumerable<T> ExecuteSqlQuery<T>(string query, params SqlParameter[] paramters)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(paramters);
                
                _context.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    T obj = default(T);
                    while (result.Read())
                    {
                        obj = Activator.CreateInstance<T>();
                        var type = typeof(T);
                        if (type.IsPrimitive)
                        {
                            obj = result.GetFieldValue<T>(0);
                        }
                        else
                        {
                            string className = type.FullName;
                            IDictionary<string, PropertyInfo> properties;
                            if(PROPERTY_CACHE.TryGetValue(className, out properties))
                            {
                                foreach(var entry in properties)
                                {
                                    if (!object.Equals(result[entry.Key], DBNull.Value))
                                    {
                                        entry.Value.SetValue(obj, result[entry.Key], null);
                                    }

                                }
                            }
                            else
                            {
                                properties = new Dictionary<string, PropertyInfo>();
                                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                                {
                                    properties.Add(prop.Name, prop);
                                    if (!object.Equals(result[prop.Name], DBNull.Value))
                                    {
                                        prop.SetValue(obj, result[prop.Name], null);
                                    }
                                }
                                PROPERTY_CACHE.Add(className, properties);
                            }
                            
                        }
                        
                        yield return obj;
                    }
                    
                }
                _context.Database.CloseConnection();
            }
        }
    }
}
