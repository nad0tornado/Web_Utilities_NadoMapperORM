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
            var sqlServerProvider = new SqlServerProvider(connectionString);
            var dataContext = new DataContext<Test>(sqlServerProvider);
            var repositoryBase = new RepositoryBase<Test>(dataContext);

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
            var testModels = await repositoryBase.GetAllAsync();
            
            Assert.Contains(testModels, m => m.Name == testModelToAddName);
            
            // .. Get By [parameterName]
            var testModelToGetByParameter = await repositoryBase.GetSingleAsync("Id", testModelToAddId);

            Assert.Equal(testModelToAddId, testModelToGetByParameter.Id);
            Assert.Equal(testModelToAddName, testModelToGetByParameter.Name);

            // .. Update
            var testModelToUpdateName = $"sqlProviderIntegration-{Guid.NewGuid()}";

            var testModelToUpdate = testModelToAddGetById;
            testModelToUpdate.Name = testModelToUpdateName;

            var updatedTestModelExpectedUpdatedRowsRowCount = 1;
            var updatedTestModelUpdatedRowsCount = await repositoryBase.UpdateAsync(testModelToUpdate);
            Assert.Equal(updatedTestModelExpectedUpdatedRowsRowCount,updatedTestModelUpdatedRowsCount);

            var updatedTestModel = await repositoryBase.GetSingleAsync(testModelToUpdate.Id);
            Assert.Equal(testModelToUpdateName,updatedTestModel.Name);

            // .. Delete
            var testModelToDeleteExpectedUpdatedRowsCount = 1;
            var testModelToDeleteUpdatedRowsCount = await repositoryBase.DeleteAsync(updatedTestModel);
            Assert.Equal(testModelToDeleteExpectedUpdatedRowsCount, testModelToDeleteUpdatedRowsCount);

            var testModelDeletedNull = await repositoryBase.GetSingleAsync(updatedTestModel.Id);
            Assert.Null(testModelDeletedNull);

            return true;
        }
    }
}
