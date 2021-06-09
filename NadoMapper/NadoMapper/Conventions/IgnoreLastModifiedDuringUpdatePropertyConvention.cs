using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NadoMapper.Conventions
{
    public class IgnoreLastModifiedDuringUpdatePropertyConvention : PropertyConventionBase
    {
        public IgnoreLastModifiedDuringUpdatePropertyConvention()
        {
            PropertyName = "LastModified";
            CRUDType = CRUDType.Update;
        }
    }
}