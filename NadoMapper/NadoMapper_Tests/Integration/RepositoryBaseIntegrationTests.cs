using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NadoMapper;
using NadoMapper.Models;
using NadoMapper.SqlProvider;
using Xunit;

namespace NadoMapper_Tests.Integration
{
    public class RepositoryBaseIntegrationTests
    {
        [Fact]
        public async Task<bool> RepositoryBaseIntegrationTest()
        {
            var connectionString = "Data Source=localhost;Initial Catalog=TestDB;Integrated Security=True;";
            var repositoryBase = new RepositoryBase<Test>(connectionString);

            // .. Add
            var testModelToAddName = $"sqlProviderIntegration-{Guid.NewGuid()}";

            var testModelToAddId = await repositoryBase.AddAsync(new Test()
            {
                Name = testModelToAddName
            });

            // .. Get By Id
            var testModelToAddGetById = await repositoryBase.GetSingleAsync(testModelToAddId);
            Assert.Equal(testModelToAddName, testModelToAddGetById.Name);

            // .. Get All
            var testModelsExpectedLength = 1;
            var testModelsFirstOrDefaultExpectedName = testModelToAddName;
            var testModels = await repositoryBase.GetAllAsync();
            
            Assert.Equal(testModelsExpectedLength,testModels.Count());
            Assert.Equal(testModelsFirstOrDefaultExpectedName,testModels.FirstOrDefault().Name);
            
            // .. Get By [parameterName]
            var testModelToGetByParameterExpectedName = testModelToAddName;
            var testModelToGetByParameter = await repositoryBase.GetSingleAsync("Id", testModelToAddId);

            Assert.Equal(testModelToGetByParameterExpectedName,testModelToGetByParameter.Name);

            // .. Update
            var testModelToUpdateName = $"sqlProviderIntegration-{Guid.NewGuid()}";

            var testModelToUpdate = testModelToAddGetById;
            testModelToUpdate.Name = testModelToUpdateName;

            var updatedTestModelExpectedRowCount = 1;
            var updatedTestModelRowCount = await repositoryBase.UpdateAsync(testModelToUpdate);
            Assert.Equal(updatedTestModelExpectedRowCount,updatedTestModelRowCount);

            var updatedTestModel = await repositoryBase.GetSingleAsync(testModelToUpdate.Id);
            Assert.Equal(testModelToUpdateName,updatedTestModel.Name);

            // .. Delete
            var testModelToDeleteExpectedRowCount = 1;
            var testModelToDeleteRowCount = await repositoryBase.DeleteAsync(updatedTestModel);

            var testModelsEmptyExpectedRowCount = 0;
            var testModelsEmpty = await repositoryBase.GetAllAsync();
            Assert.Equal(testModelsEmptyExpectedRowCount, testModelsEmpty.Count());

            var testModelDeletedNull = await repositoryBase.GetSingleAsync(updatedTestModel.Id);
            Assert.Null(testModelDeletedNull);

            return true;
        }
    }
}
