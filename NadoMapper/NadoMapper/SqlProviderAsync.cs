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

        public void LoadConnectionString(string connectionString) => _connectionString = connectionString;

        public bool VerifyInitialize()
        {
            _connection = new SqlConnection(_connectionString);
            PropertyConventions = new List<PropertyConventionBase>();

            return true;
        }

        // QUERIES

        #region ExecuteScalar
        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, KeyValuePair<string,object> parameter)
            => await ExecuteScalarAsync(command, crudType, new Dictionary<string, object>() { {parameter.Key,parameter.Value} });

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, Dictionary<string, object> parameters = null)
        {
            using (var cmd = OpenConnection(command, crudType, parameters))
            {
                var data = await cmd.ExecuteScalarAsync();
                return data;
            }
        }
        #endregion

        #region ExecuteNonQuery
        public async Task<long> ExecuteNonQueryAsync(string command, CRUDType crudType, Dictionary<string, object> parameters = null)
        {
            using (var cmd = OpenConnection(command, crudType, parameters))
            {
                var rowsUpdated = await cmd.ExecuteNonQueryAsync();

                return rowsUpdated;
            }
        }
        #endregion

        #region ExecuteReader
        public async Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, KeyValuePair<string,object> parameter)
            => await ExecuteReaderAsync(command, new Dictionary<string,object>() { {parameter.Key,parameter.Value} });

        public async Task<IEnumerable<TEntity>> ExecuteReaderAsync(string command, Dictionary<string, object> parameters = null)
        {
            using (var cmd = OpenConnection(command, CRUDType.Read, parameters))
            {
                var data = await cmd.ExecuteReaderAsync();

                var models = new List<TEntity>();

                while (data.Read())
                {
                    var objectProps = new Dictionary<string, object>();

                    for (int i = 0; i < data.VisibleFieldCount; ++i)
                        objectProps.Add(data.GetName(i), data.GetValue(i));

                    //TODO: Move this to "DataContext" ... all methods should return objects here ... No generic types in SqlProvider!
                    models.Add(NadoMapper.MapPropsToSingle<TEntity>(objectProps));
                }

                cmd.Connection.Close();
                return models;
            }
        }
        #endregion

        // SQL CONNECTION

        /// <summary>
        /// Open an SQL connection to call a stored procedure. Passed parameters will be filtered depending on 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="crudType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private SqlCommand OpenConnection(string command, CRUDType crudType, Dictionary<string,object> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(command, _connection) { CommandType = CommandType.StoredProcedure };

            foreach (KeyValuePair<string,object> parameter in parameters)
            {
                if (!PropertyConventions.Any(x => x.PropertyName == parameter.Key && x.CRUDType == crudType))
                    cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            cmd.Connection.Open();

            return cmd;
        }
    }
}