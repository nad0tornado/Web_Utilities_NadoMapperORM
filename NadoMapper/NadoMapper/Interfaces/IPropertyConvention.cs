using NadoMapper.SqlProvider;
using System;
using System.Collections.Generic;
using System.Text;

namespace NadoMapper.Interfaces
{
    public interface IPropertyConvention
    {
        public string PropertyName { get; set; }
        public CRUDType CRUDType { get; set; }
    }
}
