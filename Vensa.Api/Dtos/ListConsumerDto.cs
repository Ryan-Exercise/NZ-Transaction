//  ConsumerDtoBase.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       0:29:44 13/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
namespace Vensa.Api.Dtos
{
    public class ListConsumerDto : ConsumerDto
    {
        public string MostFrequentMethod { get; set; }
        public int MethodCount { get; set; }
    }
}
