//  ISQLFactory.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       19:3:4 14/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
namespace Vensa.Api.Repositories
{
    public interface ISQLFactory
    {
        string CreateForStatField(string cond);
        string CreateForCommonField(string cond);
        string CreateForMostFrequentMethodField(string cond);
    }
}
