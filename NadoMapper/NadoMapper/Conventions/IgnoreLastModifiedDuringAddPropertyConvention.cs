using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NadoMapper.SqlProvider;

namespace NadoMapper.Conventions
{
    public class IgnoreLastModifiedDuringAddPropertyConvention : PropertyConventionBase
    {
        public IgnoreLastModifiedDuringAddPropertyConvention()
        {
            PropertyName = "LastModified";
            CRUDType = CRUDType.Create;
        }
    }
}