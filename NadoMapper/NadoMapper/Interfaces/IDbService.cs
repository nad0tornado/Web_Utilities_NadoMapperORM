using NadoMapper.Conventions;
using NadoMapper.SqlProvider;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadoMapper.Interfaces
{
    public interface IDbService<TDict> where TDict : IDictionary<string,object>, new()
    {
        public List<IPropertyConvention> PropertyConventions { get; }


        public Task<object> ExecuteScalarAsync(string command, CRUDType crudType, string parameterName, object parameterValue)
             => ExecuteScalarAsync(command, crudType, new TDict() { { parameterName, parameterValue } });

        public Task<object> ExecuteScalarAsync(string command, CRUDType crudType, IDictionary<string, object> parameters = null);


        public Task<long> ExecuteNonQueryAsync(string command, CRUDType crudType, IDictionary<string, object> parameters = null);


        public Task<IEnumerable<IDictionary<string, object>>> ExecuteReaderAsync(string command, string parameterName, object parameterValue);

        public Task<IEnumerable<IDictionary<string, object>>> ExecuteReaderAsync(string command, IDictionary<string, object> parameters = null);
    }
}
