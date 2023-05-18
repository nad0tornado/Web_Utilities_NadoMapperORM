using NadoMapper.Enums;

namespace NadoMapper.Interfaces
{
    public interface IPropertyConvention
    {
        public string PropertyName { get; set; }
        public CRUDType CRUDType { get; set; }
    }
}
