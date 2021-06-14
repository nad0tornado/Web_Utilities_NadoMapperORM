using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NadoMapper.SqlProvider;

namespace NadoMapper.Conventions
{
    public class IgnoreDateAddedDuringAddPropertyConvention : PropertyConventionBase
    {
        public IgnoreDateAddedDuringAddPropertyConvention()
        {
            PropertyName = "DateAdded";
            CRUDType = CRUDType.Create;
        }
    }
}