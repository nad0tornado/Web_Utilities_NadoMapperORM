using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NadoMapper.Enums;
using NadoMapper.Interfaces;

namespace NadoMapper.SqlProvider
{
  public sealed class SqlService : IDbService<Dictionary<string,object>>
  {
        private readonly string _connectionString;
        public List<IPropertyConvention> PropertyConventions { get; } = new List<IPropertyConvention>();

        public SqlService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, string parameterName, object parameterValue)
            => await ExecuteScalarAsync(command, crudType, new Dictionary<string, object>() { { parameterName, parameterValue } });

        public async Task<object> ExecuteScalarAsync(string command, CRUDType crudType, IDictionary<string, object> parameters = null)
        {
            using var cmd = OpenConnection(command, crudType, parameters);
            var data = await cmd.ExecuteScalarAsync();
            cmd.Connection.Close();
            return data;
        }


        public async Task<long> ExecuteNonQueryAsync(string command, CRUDType crudType, IDictionary<string, object> parameters = null)
        {
            using var cmd = OpenConnection(command, crudType, parameters);
            var rowsUpdated = await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
            return rowsUpdated;
        }


        public async Task<IEnumerable<IDictionary<string, object>>> ExecuteReaderAsync(string command, string parameterName, object parameterValue)
            => await ExecuteReaderAsync(command, new Dictionary<string, object>() { { parameterName, parameterValue } });

        public async Task<IEnumerable<IDictionary<string, object>>> ExecuteReaderAsync(string command, IDictionary<string, object> parameters = null)
        {
            using var cmd = OpenConnection(command, CRUDType.Read, parameters);
            var data = await cmd.ExecuteReaderAsync();

            var entities = new List<IDictionary<string, object>>();

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

        /// <summary>
        /// Open a SQL connection to call a stored procedure. Passed parameters will be filtered depending on the specified CRUDType
        /// </summary>
        /// <param name="command"></param>
        /// <param name="crudType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private SqlCommand OpenConnection(string command, CRUDType crudType, IDictionary<string, object> parameters = null)
        {
            var cmd = new SqlCommand(command)
            {
                CommandType = CommandType.StoredProcedure,
                Connection = new SqlConnection(_connectionString)
            };

            var parametersWithoutConvention = parameters?.Where(x => !ParameterHasConvention(x.Key, crudType));
            parametersWithoutConvention?.ToList().ForEach(p => cmd.Parameters.AddWithValue(p.Key, p.Value));

            cmd.Connection.Open();

            return cmd;
        }

        private bool ParameterHasConvention(string parameterName, CRUDType crudType)
            => PropertyConventions.Any(x => x.PropertyName == parameterName && x.CRUDType == crudType);

    }
}