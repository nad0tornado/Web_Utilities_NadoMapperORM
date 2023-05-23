using NadoMapper.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NadoMapper.Interfaces
{
    public interface IDataContext<TEntity> where TEntity: IModel, new()
    {
        public List<IPropertyConvention> PropertyConventions { get; }

        /// <summary>
        /// Execute a stored procedure by given name, and return the number of rows updated
        /// </summary>>
        public Task<long> ExecuteNonQueryAsync(string command, IDictionary<string, object> parameters = null);
        
        /// <summary>
        /// Execute a stored procedure by given name, and return an object of type <paramref name="TEntity"/>
        /// which satisfies the given parameter
        /// </summary>>
        public Task<object> ExecuteScalarAsync(string command, CRUDType crudType, string parameterName, object parameterValue)
            => ExecuteScalarAsync(command, crudType, new Dictionary<string, object>() { { parameterName, parameterValue } });

        /// <summary>
        /// Execute a stored procedure by given name, and return an object of type <paramref name="TEntity"/>
        /// which satisfies the given parameters
        /// </summary>>
        public Task<object> ExecuteScalarAsync(string command, CRUDType crudType, IDictionary<string, object> parameters = null);
        
        /// <summary>
        /// Execute a stored procedure by given name, and return a collection of objects of type <paramref name="TEntity"/>
        /// which satisfies the given parameter
        /// </summary>>
        public Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, string parameterName, object parameterValue)
            => ExecuteReaderAsync(command, new Dictionary<string, object>() { { parameterName, parameterValue } });

        /// <summary>
        /// Execute a stored procedure by given name, and return a collection of objects of type <paramref name="TEntity"/>
        /// which satisfies the given parameters
        /// </summary>>
        public Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, IDictionary<string, object> parameters = null);

        /// <summary>
        /// Retrieve all of the rows in the database for a given model.
        /// Note: Requires stored procedure "Get[modelName](s/es)" e.g. "GetTest(s)".
        /// </summary>>
        /// <param name="id"></param>
        /// <returns>All entities of type <paramref name="TEntity"/></returns>
        public Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Retrieve a single row from the database by <paramref name="id"/>.
        /// Note: Requires stored procedure "Get[modelName]ById " e.g. "GetTestById".
        /// </summary>>
        /// <param name="id"></param>
        /// <returns>An entity of type <paramref name="TEntity"/> corresponding to <paramref name="id"/></returns>
        public async Task<TEntity> GetSingleByIdAsync(long id) =>
            await GetSingleAsync("id", id);

        /// <summary>
        /// Retrieve a single row from the database by <paramref name="name"/>.
        /// Note: Requires stored procedure "Get[modelName]ByName " e.g. "GetTestByName".
        /// </summary>>
        /// <param name="name"></param>
        /// <returns>An entity of type <paramref name="TEntity"/> corresponding to <paramref name="name"/></returns>
        public async Task<TEntity> GetSingleByNameAsync(string name) =>
            await GetSingleAsync("name", name);

        /// <summary>
        /// Retrieve a single row from the database based on some parameter (<paramref name="parameterName"/> and <paramref name="parameterValue"/>)
        /// Note: Requires stored procedure "Get[modelName]By[parameterName]" e.g. "GetTestByName".
        /// </summary>>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <returns>An entity of type <paramref name="TEntity"/> corresponding to <paramref name="parameterName"/></returns>
        public Task<TEntity> GetSingleAsync(string parameterName, object parameterValue);

        /// <summary>
        /// Create a new row in the database containing the parameters of <paramref name="model" /> and return it's id.
        /// Note: Requires stored procedure "Add[modelName]" e.g. "AddTest"
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="T:System.ArgumentException"/>
        /// <returns>An id of type <see cref="T:System.Int64"/></returns>
        public Task<long> AddAsync(TEntity model);

        /// <summary>
        /// Update a row (or rows) in the database based on the parameters specified in <paramref name="model" /> and return the number of rows that were updated.
        /// Note: Requires stored procedure "Update[modelName] e.g. "UpdateTest"
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Number of rows updated as a <see cref="T:System.Int64"/></returns>
        public Task<long> UpdateAsync(TEntity model);

        /// <summary>
        /// Delete a row from the database corresponding to the <paramref name="Id"/> and <paramref name="LastModified"/> of <paramref name="model"/>
        /// Note: Requires stored procedure "Delete[modelName] e.g. "DeleteTest"
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Number of rows updated as a <see cref="T:System.Int64"/></returns>
        public Task<long> DeleteAsync(TEntity model);
    }
}
