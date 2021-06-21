using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using NadoMapper.Models;
using NadoMapper.Conventions;
using Pluralize;
using Pluralize.NET;

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
        }

        public Task<IEnumerable<TEntity>> GetAllAsync() => _dataContext.GetAllAsync();

        public Task<TEntity> GetSingleAsync(string parameterName, object parameterValue) => _dataContext.GetSingleAsync(parameterName, parameterValue);

        public Task<TEntity> GetSingleAsync(long id) => _dataContext.GetSingleByIdAsync(id);

        public Task<long> AddAsync(TEntity item) => _dataContext.AddAsync(item);

        public Task<long> UpdateAsync(TEntity item) => _dataContext.UpdateAsync(item);

        public Task<long> DeleteAsync(TEntity item) => _dataContext.DeleteAsync(item);
    }
}