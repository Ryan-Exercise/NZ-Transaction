//  SQLFactory.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       19:4:23 14/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Vensa.Api.Repositories
{
    public class SQLFactory : ISQLFactory
    {
        private string _commonFieldQuery;
        private string _statFieldQuery;
        private string _mostFrequentMethodQuery;

        public string CreateForCommonField(string cond)
        {
            StringBuilder builder = new();
            builder.Append("DECLARE @IdPage TABLE(ConsumerId BIGINT PRIMARY KEY);");
            builder.Append("INSERT INTO @IdPage SELECT Id FROM Consumer ");
            if (!string.IsNullOrEmpty(cond))
            {
                builder.Append($"WHERE {cond} ");
            }

            if (string.IsNullOrEmpty(_commonFieldQuery))
            {
                _commonFieldQuery = LoadFromFile("CommenFieldQuery.sql");
            }
            builder.Append(_commonFieldQuery);
            return builder.ToString();
        }

        public string CreateForStatField(string cond)
        {
            StringBuilder builder = new();
            builder.Append("DECLARE @Page TABLE(ConsumerId BIGINT PRIMARY KEY,");
            builder.Append("Balance DECIMAL(18,2), LastTransactionTime DATETIME);");
            builder.Append("INSERT INTO @Page SELECT * FROM (SELECT t1.Id AS ConsumerId,");
            builder.Append("SUM(t2.[Value]) AS Balance, MAX(t2.[DateTime]) AS LastTransactionTime ");
            builder.Append("FROM Consumer t1 ");
            builder.Append("INNER JOIN [Transaction] t2 ON t2.ConsumerId = t1.Id GROUP BY t1.Id) t3 ");

            if (!string.IsNullOrEmpty(cond))
            {
                builder.Append($"WHERE {cond} ");
            }
            if (string.IsNullOrEmpty(_statFieldQuery))
            {
                
                _statFieldQuery = LoadFromFile("StatFieldQuery.sql");
            }
            builder.Append(_statFieldQuery);
            return builder.ToString() ;

        }

        public string CreateForMostFrequentMethodField(string cond)
        {
            StringBuilder builder = new();
            builder.Append("SELECT t1.ConsumerId,t1.TransactionMethod,COUNT(t1.TransactionMethod) MethodCount,");
            builder.Append("MAX(t1.[DateTime]) as LastTransactionTime ");
            builder.Append("INTO #Method FROM [Transaction] t1 ");
            builder.Append("GROUP BY t1.ConsumerId, t1.TransactionMethod;");
            builder.Append("DECLARE @MethodMax TABLE(ConsumerId BIGINT,  MethodId INT,");
            builder.Append("MethodCount INT, LastTransactionTime DATETIME);");
            builder.Append("INSERT INTO @MethodMax SELECT t2.ConsumerId,");
            builder.Append("t3.TransactionMethod MethodId, t2.MethodCount, t3.LastTransactionTime ");
            builder.Append("FROM (SELECT t1.ConsumerId, MAX(t1.MethodCount) MethodCount ");
            builder.Append("FROM #Method t1 GROUP BY t1.ConsumerId) t2 ");
            builder.Append("JOIN #Method t3 ON t3.ConsumerId = t2.ConsumerId ");
            builder.Append("AND t3.MethodCount = t2.MethodCount ");

            if (!string.IsNullOrEmpty(cond))
            {
                builder.Append($"And {cond} ");
            }

            if (string.IsNullOrEmpty(_mostFrequentMethodQuery))
            {
                _mostFrequentMethodQuery = LoadFromFile("MostFrequentMethodQuery.sql");
            }
            builder.Append(_mostFrequentMethodQuery);
            
            
            return builder.ToString();

            /*
              



 

  


             */
        }

        private string LoadFromFile(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullName = $"{assembly.GetName().Name}.Sql.{fileName}";

            using (var stream = assembly.GetManifestResourceStream(fullName))
            {
                if(stream != null)
                {
                    return new StreamReader(stream).ReadToEnd();
                }
            }
            return string.Empty;
        }
    }
}
