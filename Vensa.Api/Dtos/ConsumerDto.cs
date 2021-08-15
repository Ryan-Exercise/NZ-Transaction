//  ConsumerDto.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       11:47:40 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
namespace Vensa.Api.Dtos
{
    public class ConsumerDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public DateTime LastTransactionTime { get; set; }
        public decimal Balance { get; set; }
    }
}
