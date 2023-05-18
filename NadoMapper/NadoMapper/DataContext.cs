﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NadoMapper.Enums;
using NadoMapper.Interfaces;
using NadoMapper.Models;
using Pluralize.NET;

namespace NadoMapper
{
  /// <summary>
  /// A generalised, standalone wrapper which gives the ability to perform basic CRUD operations on a database.
  /// The DataContext may be included as a member of a parent "Repository" class to provide specific functionality.
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  public class DataContext<TEntity> : IDataContext<TEntity,Dictionary<string,object>> where TEntity : ModelBase, new()
  {
        private readonly IDbService<Dictionary<string, object>> _databaseService;

        public List<IPropertyConvention> PropertyConventions => _databaseService.PropertyConventions;

        private string modelName => typeof(TEntity).Name;
        private string modelNamePlural { get; }

        /// <summary>
        /// Create a new DataContext accepting a <paramref name="connectionString"/> for use with models of type <paramref name="TEntity"/>
        /// </summary>
        /// <param name="connectionString"></param>
        public DataContext(IDbService<Dictionary<string, object>> databaseService)
        {
            _databaseService = databaseService;

            var pluralizer = new Pluralizer();
            modelNamePlural = pluralizer.Pluralize(modelName);
        }

        /// <summary>
        /// Execute a stored procedure by given name, and return the number of rows updated
        /// </summary>>
        public Task<long> ExecuteNonQueryAsync(string command, IDictionary<string, object> parameters = null)
         => _databaseService.ExecuteNonQueryAsync(command, CRUDType.None, parameters);

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
        public Task<object> ExecuteScalarAsync(string command, CRUDType crudType, IDictionary<string, object> parameters = null)
          => _databaseService.ExecuteScalarAsync(command, crudType, parameters);

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
        public async Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, IDictionary<string, object> parameters = null)
        {
            var data = await _databaseService.ExecuteReaderAsync(command, parameters);
            return data.Select(d => NadoMapper.MapPropsToSingle<TEntity>(d));
        }

        /// <summary>
        /// Retrieve all of the rows in the database for a given model.
        /// Note: Requires stored procedure "Get[modelName](s/es)" e.g. "GetTest(s)".
        /// </summary>>
        /// <param name="id"></param>
        /// <returns>All entities of type <paramref name="TEntity"/></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var data = await _databaseService.ExecuteReaderAsync($"Get{modelNamePlural}");

            return data.Select(d => NadoMapper.MapPropsToSingle<TEntity>(d));
        }

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
        public async Task<TEntity> GetSingleAsync(string parameterName, object parameterValue)
        {
            var procName = $"Get{modelName}By{parameterName.ToUpper()[0] + parameterName.Substring(1)}";

            var data = await _databaseService.ExecuteReaderAsync(procName, parameterName, parameterValue);
            var single = data.FirstOrDefault();

            return NadoMapper.MapSingle<TEntity>(single);
        }

        /// <summary>
        /// Create a new row in the database containing the parameters of <paramref name="model" /> and return it's id.
        /// Note: Requires stored procedure "Add[modelName]" e.g. "AddTest"
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="T:System.ArgumentException"/>
        /// <returns>An id of type <see cref="T:System.Int64"/></returns>
        public async Task<long> AddAsync(TEntity model)
        {
            var parameters = NadoMapper.ReflectPropsFromSingle(model);
            var id = await _databaseService.ExecuteScalarAsync($"Add{modelName}", CRUDType.Create, parameters);

            return Convert.ToInt64(id);
        }

        /// <summary>
        /// Update a row (or rows) in the database based on the parameters specified in <paramref name="model" /> and return the number of rows that were updated.
        /// Note: Requires stored procedure "Update[modelName] e.g. "UpdateTest"
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Number of rows updated as a <see cref="T:System.Int64"/></returns>
        public async Task<long> UpdateAsync(TEntity model)
            => await _databaseService.ExecuteNonQueryAsync($"Update{modelName}", CRUDType.Update, NadoMapper.ReflectPropsFromSingle(model));

        /// <summary>
        /// Delete a row from the database corresponding to the <paramref name="Id"/> and <paramref name="LastModified"/> of <paramref name="model"/>
        /// Note: Requires stored procedure "Delete[modelName] e.g. "DeleteTest"
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Number of rows updated as a <see cref="T:System.Int64"/></returns>
        public async Task<long> DeleteAsync(TEntity model)
        => await _databaseService.ExecuteNonQueryAsync($"Delete{modelName}", CRUDType.Update, new Dictionary<string, object>()
        {
            {"id",model.Id},
            {"lastModified",model.LastModified}
        });
    }
}