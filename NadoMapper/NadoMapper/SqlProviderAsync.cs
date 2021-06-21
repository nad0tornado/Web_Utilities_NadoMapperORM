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

namespace NadoMapper.SqlProvider
{
    /// <summary>
    /// Dictates the type of CRUD operation being performed, and therefore which parameters to keep or omit depending on selected property conventions
    /// </summary>
    public enum CRUDType
    {
        Create,
        Read,
        Update,
        Delete
    }

    public sealed class SqlProviderAsync
    {
        private string _connectionString;
        public List<PropertyConventionBase> PropertyConventions;

        public SqlProviderAsync(string connectionString)
        {
            _connectionString = connectionString;
            PropertyConventions = new List<PropertyConventionBase>();
        }

        // QUERIES

        #region ExecuteScalar
        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, string parameterName, object parameterValue)
            => await ExecuteScalarAsync(command, crudType, new Dictionary<string, object>() { {parameterName,  parameterValue} });

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
        public async Task<IEnumerable<Dictionary<string, object>>> ExecuteReaderAsync(string command, string parameterName, object parameterValue)
            => await ExecuteReaderAsync(command, new Dictionary<string,object>() { { parameterName, parameterValue } });

        public async Task<IEnumerable<Dictionary<string,object>>> ExecuteReaderAsync(string command, Dictionary<string, object> parameters = null)
        {
            using (var cmd = OpenConnection(command, CRUDType.Read, parameters))
            {
                var data = await cmd.ExecuteReaderAsync();

                var entities = new List<Dictionary<string, object>>();

                while (data.Read())
                {
                    var objectProps = new Dictionary<string, object>();

                    for (int i = 0; i < data.VisibleFieldCount; ++i)
                        objectProps.Add(data.GetName(i), data.GetValue(i));

                    entities.Add(objectProps);
                }

                cmd.Connection.Close();
                return entities;
            }
        }
        #endregion

        // SQL CONNECTION

        /// <summary>
        /// Open a SQL connection to call a stored procedure. Passed parameters will be filtered depending on the specified CRUDType
        /// </summary>
        /// <param name="command"></param>
        /// <param name="crudType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private SqlCommand OpenConnection(string command, CRUDType crudType, Dictionary<string,object> parameters = null)
        {
            SqlCommand cmd = new SqlCommand(command) { CommandType = CommandType.StoredProcedure };
            cmd.Connection = new SqlConnection(_connectionString);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (!PropertyConventions.Any(x => x.PropertyName == parameter.Key && x.CRUDType == crudType))
                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }

            cmd.Connection.Open();

            return cmd;
        }
    }
}