using Tobasco.Enums;
using Tobasco.Properties;

namespace Tobasco.Factories
{
    public class DapperPropertyFactory : PropertyFactory
    {
        protected override string GetPropertyTemplate(Datatype datatype)
        {
            switch (datatype)
            {
                case Datatype.ChildCollection:
                    return Resources.PropertyChildCollection;
                case Datatype.ReadOnlyGuid:
                    return Resources.PropertyReadonlyGuid;
                default:
                    return Resources.PropertyDapperDefault;
            }
        }
    }
}
