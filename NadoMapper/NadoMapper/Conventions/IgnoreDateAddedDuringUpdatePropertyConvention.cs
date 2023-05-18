using NadoMapper.Enums;

namespace NadoMapper.Conventions
{
    public class IgnoreDateAddedDuringUpdatePropertyConvention : PropertyConventionBase
    {
        public IgnoreDateAddedDuringUpdatePropertyConvention()
        {
            PropertyName = "DateAdded";
            CRUDType = CRUDType.Update;
        }
    }
}