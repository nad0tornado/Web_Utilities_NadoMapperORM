using NadoMapper.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace NadoMapper_Tests.Unit
{
    public class NadoMapperUnitTests
    {
        [Fact]
        public void ReflectPropsFromSingleUnitTest()
        {
            var dateTimeNow = DateTime.Now;
            var testModelLastModifiedExpected = BitConverter.GetBytes(dateTimeNow.Ticks);

            var testModelPropsExpected = new Dictionary<string, object>()
            {
                {"Id", 1},
                {"Name", "Test"},
                {"DateAdded", dateTimeNow},
                {"LastModified", testModelLastModifiedExpected}
            };

            var testModelToReflect = new Test()
            {
                Id = (int) testModelPropsExpected["Id"],
                Name = testModelPropsExpected["Name"].ToString(),
                DateAdded = (DateTime) testModelPropsExpected["DateAdded"],
                LastModified = testModelLastModifiedExpected
            };

            var testModelPropsActual = NadoMapper.NadoMapper.ReflectPropsFromSingle(testModelToReflect);

            Assert.Equal(testModelPropsExpected["Id"], testModelPropsActual["Id"]);
            Assert.Equal(testModelPropsExpected["Name"], testModelPropsActual["Name"]);
            Assert.Equal(testModelPropsExpected["DateAdded"], testModelPropsActual["DateAdded"]);
            Assert.Equal(testModelPropsExpected["LastModified"], testModelPropsActual["LastModified"]);
        }
    }
}
