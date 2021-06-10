using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public enum CRUDType
    {
        Create,
        Read,
        Update,
        Delete
    }

    public struct NadoMapperParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    public class SqlProviderAsync<TEntity> where TEntity : ModelBase, new()
    {
        private SqlConnection _connection;
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

        /*public async Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, NadoMapperParameter parameter)
            => await ExecuteReaderAsync(command, new List<NadoMapperParameter>() { parameter });*/

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, crudType, CommandType.StoredProcedure, parameters);

            var data = await cmd.ExecuteScalarAsync();

            cmd.Connection.Close();
            return data;
        }

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, NadoMapperParameter parameter)
            => await ExecuteScalarAsync(command, crudType, new List<NadoMapperParameter>() { parameter });

        public async Task<long> ExecuteNonQueryAsync(string command, CRUDType crudType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            var cmd = OpenConnection(command, crudType, CommandType.StoredProcedure, parameters);

            var rowsUpdated = await cmd.ExecuteNonQueryAsync();

            cmd.Connection.Close();
            return rowsUpdated;
        }

        private SqlCommand OpenConnection(string command, CRUDType crudType, NadoMapperParameter parameter)
            => OpenConnection(command, crudType, CommandType.StoredProcedure, new List<NadoMapperParameter>() { parameter });

        private SqlCommand OpenConnection(string command, CRUDType crudType, CommandType commandType, IEnumerable<NadoMapperParameter> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(command, _connection) { CommandType = commandType };
            // .. how do we initialise the SqlCommand object with a connection string?
            // .. can we just pass a string or do we need to pass a new connection every time?

            if (parameters != null)
            {
                foreach (NadoMapperParameter parameter in parameters)
                {
                    if (!PropertyConventions.Any(x => x.PropertyName == parameter.Name && x.CRUDType == crudType))
                        cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
                }
            }

            cmd.Connection.Open();

            return cmd;
        }
    }
}