using System.Collections.Generic;
using System.Threading.Tasks;
using NadoMapper.Models;
using NadoMapper.Conventions;
using NadoMapper.SqlProvider;

namespace NadoMapper
{
  /// <summary>
  /// Implements the "DataContext" class to expose public methods to perform basic CRUD (Get, Add, Update, Delete) on an SQL Database. This class can be inherited by several repository classes
  /// in a larger project that have been configured for a particular purpose
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  public class RepositoryBase<TEntity> where TEntity : ModelBase, new()
  {
    private readonly DataContext<TEntity> _dataContext;

    public List<PropertyConventionBase> PropertyConventions => _dataContext.PropertyConventions;

    public RepositoryBase(string connectionString)
    {
      _dataContext = new DataContext<TEntity>(connectionString);

      _dataContext.PropertyConventions.Add(new IgnoreDateAddedDuringAddPropertyConvention());
      _dataContext.PropertyConventions.Add(new IgnoreLastModifiedDuringAddPropertyConvention());
      _dataContext.PropertyConventions.Add(new IgnoreDateAddedDuringUpdatePropertyConvention());
      _dataContext.PropertyConventions.Add(new IgnoreIdDuringAddPropertyConvention());
    }

    /// <summary>
    /// Execute a stored procedure by given name, and return the number of rows updated
    /// </summary>>
    public Task<long> ExecuteNonQueryAsync(string command, CRUDType crudType, Dictionary<string, object> parameters = null)
     => _dataContext.ExecuteNonQueryAsync(command, crudType, parameters);

    /// <summary>
    /// Execute a stored procedure by given name, and return an object of type <paramref name="TEntity"/>
    /// which satisfies the given parameter
    /// </summary>>
    public Task<object> ExecuteScalarAsync(string command, CRUDType crudType, string parameterName, object parameterValue)
        => ExecuteScalarAsync(command, crudType, parameterName, parameterValue);

    /// <summary>
    /// Execute a stored procedure by given name, and return an object of type <paramref name="TEntity"/>
    /// which satisfies the given parameters
    /// </summary>>
    public Task<object> ExecuteScalarAsync(string command, CRUDType crudType, Dictionary<string, object> parameters = null)
        => _dataContext.ExecuteScalarAsync(command, crudType, parameters);

    /// <summary>
    /// Execute a stored procedure by given name, and return a collection of objects of type <paramref name="TEntity"/>
    /// which satisfies the given parameter
    /// </summary>>
    public Task<IEnumerable<Dictionary<string, object>>> ExecuteReaderAsync(string command, string parameterName, object parameterValue)
        => ExecuteReaderAsync(command, new Dictionary<string, object>() { { parameterName, parameterValue } });

    /// <summary>
    /// Execute a stored procedure by given name, and return a collection of objects of type <paramref name="TEntity"/>
    /// which satisfies the given parameters
    /// </summary>>
    public Task<IEnumerable<Dictionary<string, object>>> ExecuteReaderAsync(string command, Dictionary<string, object> parameters = null)
        => _dataContext.ExecuteReaderAsync(command, parameters);

    public Task<IEnumerable<TEntity>> GetAllAsync() => _dataContext.GetAllAsync();

    public Task<TEntity> GetSingleAsync(string parameterName, object parameterValue) => _dataContext.GetSingleAsync(parameterName, parameterValue);

    public Task<TEntity> GetSingleAsync(long id) => _dataContext.GetSingleByIdAsync(id);

    public Task<long> AddAsync(TEntity item) => _dataContext.AddAsync(item);

    public Task<long> UpdateAsync(TEntity item) => _dataContext.UpdateAsync(item);

    public Task<long> DeleteAsync(TEntity item) => _dataContext.DeleteAsync(item);
  }
}