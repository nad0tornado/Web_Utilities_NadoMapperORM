using NadoMapper.Enums;

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