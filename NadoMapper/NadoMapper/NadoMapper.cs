using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NadoMapper
{
    public class NadoMapper
    {
        public static TEntity MapPropsToSingle<TEntity>(Dictionary<string, object> props) =>
            JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(props));

        public static TEntity MapSingle<TEntity>(object model) =>
            JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(model));

        protected Dictionary<string,object> ReflectPropsFromSingle<TEntity>(TEntity entity)
        {
            var parameters = new Dictionary<string, object>();

            foreach (PropertyInfo prop in entity.GetType().GetProperties())
                parameters.Add(prop.Name, prop.GetValue(entity));

            return parameters;
        }
    }
}
