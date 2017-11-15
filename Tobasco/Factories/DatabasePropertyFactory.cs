using Tobasco.Enums;
using Tobasco.Model;
using Tobasco.Model.DatabaseProperties;

namespace Tobasco.Factories
{
    public static class DatabasePropertyFactory
    {
        public static DatabaseProperty GetDatabaseProperty(Property propery)
        {
            switch (propery.DataType.Datatype)
            {
                case Datatype.String:
                    return new StringProperty(propery);
                case Datatype.Decimal:
                    return new DecimalProperty(propery);
                default:
                    return new DatabaseProperty(propery);
            }
        }
    }
}
