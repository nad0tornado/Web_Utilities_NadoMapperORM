using NadoMapper.Enums;

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