using NadoMapper.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NadoMapper.Interfaces
{
    public interface IRepository<TEntity> where TEntity: IModel, new()
    {
        public List<IPropertyConvention> PropertyConventions { get; }

        /// <summary>
        /// Execute a stored procedure by given name and parameter, and return the number of rows updated
        /// </summary>>
        public Task<long> ExecuteNonQueryAsync(string command, string parameterName, object parameterValue)
         => ExecuteNonQueryAsync(command, new Dictionary<string, object>() { { parameterName, parameterValue } });

        /// <summary>
        /// Execute a stored procedure by given name and parameters, and return the number of rows updated
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


        public Task<IEnumerable<TEntity>> GetAllAsync();

        public Task<TEntity> GetSingleAsync(string parameterName, object parameterValue);

        public Task<TEntity> GetSingleAsync(long id);

        public Task<long> AddAsync(TEntity item);

        public Task<long> UpdateAsync(TEntity item);

        public Task<long> DeleteAsync(TEntity item);
    }
}
