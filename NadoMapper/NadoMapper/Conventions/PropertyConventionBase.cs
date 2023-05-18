using NadoMapper.Enums;
using NadoMapper.Interfaces;

namespace NadoMapper.Conventions
{
    public class PropertyConventionBase : IPropertyConvention
    {
        public string PropertyName { get; set; }
        public CRUDType CRUDType { get; set; }
    }
}