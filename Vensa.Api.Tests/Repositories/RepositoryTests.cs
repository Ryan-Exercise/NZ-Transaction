//  RepositoryTests.cs
//
// Description: 
//       <Describe here>
//  Author:
//       xuchunlei <hitxcl@gmail.com>
//  Create at:
//       13:17:40 12/8/2021
//
//  Copyright (c) 2021 ${CopyrightHolder}
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Moq;
using Vensa.Api.Entities;
using Vensa.Api.Extensions;
using Vensa.Api.Repositories;
using Vensa.Api.Tests.Base;
using Xunit;

namespace Vensa.Api.Tests.Repositories
{
    public class RepositoryTests : DbContextBaseTests
    {
        [Fact]
        public void GetAll_CustomerCondExpression_ReturnsConsumerEntity()
        {
            using(var context = new VensaContext(Options))
            {
                // Arrange
                const string FIRST_NAME = "Richie";
                const string LAST_NAME = "Hobkirk";
                Expression<Func<Consumer, bool>> firstNameExp =
                c => c.FirstName.ToLower().Equals(FIRST_NAME.ToLower());
                Expression<Func<Consumer, bool>> lastNameExp =
                c => c.LastName.ToLower().Equals(LAST_NAME.ToLower());
                var logger = new Mock<ILogger<Repository<Consumer>>>();
                var repository = new Repository<Consumer>(context, logger.Object);

                //Action
                var actual = repository.GetAll(firstNameExp.And(lastNameExp))
                    .AsEnumerable().FirstOrDefault();

                // Assert
                Assert.Equal(FIRST_NAME.ToLower(), actual.FirstName.ToLower());
            }
            
        }
    }
}
