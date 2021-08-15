//  TransactionDto.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       11:47:29 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
namespace Vensa.Api.Dtos
{
    public class TransactionDto
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public string TransactionType { get; set; }
        public string TransactionMethod { get; set; }
        public decimal Value { get; set; }
    }
}
