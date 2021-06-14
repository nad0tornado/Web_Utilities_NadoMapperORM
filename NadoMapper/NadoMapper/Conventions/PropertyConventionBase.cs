using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NadoMapper.SqlProvider;

namespace NadoMapper.Conventions
{
    public class PropertyConventionBase
    {
        public string PropertyName { get; set; }
        public CRUDType CRUDType { get; set; }
    }
}