using NadoMapper.Enums;

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