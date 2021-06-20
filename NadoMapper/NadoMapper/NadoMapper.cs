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

        /// <summary>
        /// Retrieve the properties of a given model using reflection and return them
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns>A Dictionary (string,object) consisting of the properties of <paramref name="entity"/> </returns>
        public static Dictionary<string,object> ReflectPropsFromSingle<TEntity>(TEntity entity) where TEntity: new()
        {
            var parameters = new Dictionary<string, object>();

            foreach (PropertyInfo prop in entity.GetType().GetProperties())
                parameters.Add(prop.Name, prop.GetValue(entity));

            return parameters;
        }
    }
}
