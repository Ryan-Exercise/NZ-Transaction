//  MultipleEntitiesFoundException.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       14:47:58 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
namespace Vensa.Api.Exceptions
{
    public class MultipleEntitiesFoundException : Exception
    {
        private readonly string _entity;
        public MultipleEntitiesFoundException(string entity)
        {
            _entity = entity;
        }

        public override string Message => $"More than 1 {_entity}s found";
    }
}
