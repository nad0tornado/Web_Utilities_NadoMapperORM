using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}
