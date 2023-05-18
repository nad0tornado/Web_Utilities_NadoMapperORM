using NadoMapper.Enums;

namespace NadoMapper.Conventions
{
    public class IgnoreIdDuringAddPropertyConvention : PropertyConventionBase
    {
        public IgnoreIdDuringAddPropertyConvention()
        {
            PropertyName = "Id";
            CRUDType = CRUDType.Create;
        }
    }
}