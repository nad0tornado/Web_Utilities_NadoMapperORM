using System;
using System.Collections.Generic;
// using System.Data;
// using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
// using System.Web;
using NadoMapper.Models;
using NadoMapper.Conventions;
using Newtonsoft.Json;
using Pluralize.NET;

namespace NadoMapper
{
    public class DataContext<TEntity> where TEntity: ModelBase, new()
    {
        /*private SqlConnection _connection;
        private string _connectionString;
        public List<PropertyConventionBase> PropertyConventions;

        private Pluralizer _pluralizer;
        private string _modelName => typeof(TEntity).Name;
        private string _modelNamePlural => _pluralizer.Pluralize(_modelName);

        public void LoadConnectionString(string connectionString) => _connectionString = connectionString;

        public bool VerifyInitialize()
        {
            _connection = new SqlConnection(_connectionString);
            _pluralizer = new Pluralizer();
            PropertyConventions = new List<PropertyConventionBase>();

            return true;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await ExecuteReaderAsync($"Get{_modelNamePlural}");

        public async Task<TEntity> GetSingleAsync(NadoMapperParameter parameter)
        {
            var parameterName = parameter.Name[0] + parameter.Name.Substring(1);

            var cmd = OpenConnection($"Get{_modelName}By{parameterName}", CRUDType.Read, parameter);

            var data = await cmd.ExecuteScalarAsync();

            return NadoMapper.MapSingle(data);
        }

        public async Task<TEntity> GetSingleAsync(long id) =>
            (await ExecuteReaderAsync($"Get{_modelName}ById", new NadoMapperParameter() { Name = "id", Value = id }))
            .FirstOrDefault();

        public async Task<TEntity> GetSingleByNameAsync(string name)
        {
            var cmd = OpenConnection($"Get{_modelName}ByName", CRUDType.Read, new NadoMapperParameter() { Name = "name", Value = name });

            var data = await cmd.ExecuteScalarAsync();

            return MapSingle(data);
        }

        public async Task<TEntity> GetSingleAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(procName, CRUDType.Read, CommandType.StoredProcedure, parameters);

            var data = await cmd.ExecuteScalarAsync();

            return MapSingle(data);
        }

        public async Task<TEntity> AddAsync(TEntity model)
        {
            var cmd = OpenConnection($"Add{_modelName}", CRUDType.Create, CommandType.StoredProcedure, GetParamsFromModel(model));
            var id = await cmd.ExecuteScalarAsync();
            cmd.Connection.Close();

            cmd = OpenConnection($"SELECT * from {_modelNamePlural} where Id={id}", CRUDType.Read, CommandType.Text);
            var data = await cmd.ExecuteReaderAsync();

            data.Read();

            var objectProps = new Dictionary<string, object>();

            for (int i = 0; i < data.VisibleFieldCount; ++i)
                objectProps.Add(data.GetName(i), data.GetValue(i));

            cmd.Connection.Close();

            return MapPropsToSingle(objectProps);
        }

        public async Task<long> UpdateAsync(TEntity model) => await ExecuteNonQueryAsync($"Update{_modelName}", CRUDType.Update, GetParamsFromModel(model));

        public async Task<long> DeleteAsync(TEntity model)
            => await ExecuteNonQueryAsync($"Delete{_modelName}", CRUDType.Update, new List<NadoMapperParameter>()
            {
                new NadoMapperParameter(){Name="id",Value=model.Id},
                new NadoMapperParameter(){Name="lastModified",Value=model.LastModified}
            }); */
    }
}