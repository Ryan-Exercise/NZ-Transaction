//  EntityNotFoundException.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       14:41:47 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
namespace Vensa.Api.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private readonly string _entity;
        public EntityNotFoundException(string entity)
        {
            _entity = entity;
        }

        public override string Message => $"{_entity} not found";
    }
}
