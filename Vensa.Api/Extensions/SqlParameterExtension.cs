//  SqlParameterExtension.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       13:10:13 14/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace Vensa.Api.Extensions
{
    public static class SqlParameterExtension
    {
        private static readonly Regex DigitsOnly = new Regex(@"[^\d]+");
        private static readonly ISet<string> EXACT_SEARCH_FIELDS = new HashSet<string>
        {
            "Id",
            "mobile",
            "dateofbirth",
            "lasttransactiontime",
            "balance",
            "mostfrequentmethod"
        };
        private static readonly ISet<string> FUZZY_SEARCH_FIELDS = new HashSet<string>
        {
            "firstname",
            "middlename",
            "lastname",
            "preferredname",
        };


        public static string ToConditions(this SqlParameter param)
        {
            var field = param.ParameterName.Substring(1).ToString();
            
            if (FUZZY_SEARCH_FIELDS.Contains(field))
            {
                return $"{field} Like '%' + @{field} + '%'";
            }

            if (EXACT_SEARCH_FIELDS.Contains(field))
            {
                if ("dateofbirth".Equals(field) || "lasttransactiontime".Equals(field))
                {
                    return $"CONVERT(DATE, {field}) = CONVERT(DATE, @{field})";
                }
                if ("mobile".Equals(field))
                {
                    param.Value = "+64" + DigitsOnly.Replace(param.Value.ToString(), "");
                }
                if ("mostfrequentmethod".Equals(field))
                {
                    if (!string.IsNullOrEmpty(param.Value.ToString())){
                        return $"t3.TransactionMethod = @{field}";
                    }
                }
                return $"{field} = @{field}";
            }
            return string.Empty;
        }
    }
}
