using System;
using System.Collections.Generic;
using System.Data;
// using System.Data;
// using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
// using System.Web;
using NadoMapper.Models;
using NadoMapper.Conventions;
using NadoMapper.SqlProvider;
using Newtonsoft.Json;
using Pluralize.NET;

namespace NadoMapper
{
    public class DataContext<TEntity> : IDisposable where TEntity: ModelBase, new()
    {
        private SqlProviderAsync _sqlProviderAsync;

        private Pluralizer _pluralizer;
        private string _modelName => typeof(TEntity).Name;
        private string _modelNamePlural => _pluralizer.Pluralize(_modelName);

        public void LoadConnectionString(string connectionString) => _sqlProviderAsync.LoadConnectionString(connectionString);

        public bool VerifyInitialize()
        {
            _pluralizer = new Pluralizer();
            _sqlProviderAsync = new SqlProviderAsync();

            _sqlProviderAsync.VerifyInitialize();

            return true;
        }
        public void Dispose()
        {
            _sqlProviderAsync.Dispose();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var data = await _sqlProviderAsync.ExecuteReaderAsync($"Get{_modelNamePlural}");

            return data.Select(d => NadoMapper.MapPropsToSingle<TEntity>(d));
        }

        public async Task<TEntity> GetSingleByIdAsync(long id) =>
            await GetSingleAsync(new NadoMapperParameter() {Name = "id", Value = id});

        public async Task<TEntity> GetSingleByNameAsync(string name) =>
            await GetSingleAsync(new NadoMapperParameter() {Name = "name", Value = name});

        public async Task<TEntity> GetSingleAsync(NadoMapperParameter parameter)
        {
            var parameterName = parameter.Name.ToUpper()[0] + parameter.Name.Substring(1);
            var procName = $"Get{_modelName}By{parameterName}";

            var data = await _sqlProviderAsync.ExecuteReaderAsync(procName, parameter.Name, parameter.Value);
            var single = data.FirstOrDefault();

            return NadoMapper.MapSingle<TEntity>(single);
        }

        public async Task<long> AddAsync(TEntity model)
        {
            var parameters = NadoMapper.ReflectPropsFromSingle(model);
            var id = await _sqlProviderAsync.ExecuteScalarAsync($"Add{_modelName}", CRUDType.Create, parameters);

            if(id.GetType() != typeof(long))
                throw new ApplicationException($"Expected a long to be returned, got {id}");

            return (long)id;
        }

        /*public async Task<long> UpdateAsync(TEntity model) => await ExecuteNonQueryAsync($"Update{_modelName}", CRUDType.Update, GetParamsFromModel(model));

        public async Task<long> DeleteAsync(TEntity model)
            => await ExecuteNonQueryAsync($"Delete{_modelName}", CRUDType.Update, new List<NadoMapperParameter>()
            {
                new NadoMapperParameter(){Name="id",Value=model.Id},
                new NadoMapperParameter(){Name="lastModified",Value=model.LastModified}
            }); */
    }
}