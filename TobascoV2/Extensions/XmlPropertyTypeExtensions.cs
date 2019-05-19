using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Context;
using TobascoV2.Enums;

namespace TobascoV2.Extensions
{
    public static class XmlPropertyTypeExtensions
    {
        public static string GetSqlDataType(this XmlPropertyType property)
        {
            switch (property.Name)
            {
                case Datatype.Boolean:
                    return "bit";
                case Datatype.String:
                    if (property.DbType == DataDbType.Varchar ||
                        property.DbType == DataDbType.Nvarchar ||
                        property.DbType == DataDbType.Char)
                    {
                        return $"{property.DbType.GetDescription()}({property.Size})";
                    }
                    return $"nvarchar({property.Size})";
                case Datatype.Int:
                    return "int";
                case Datatype.Decimal:
                    if (property.DbType == DataDbType.Smallmoney || property.DbType == DataDbType.Money)
                    {
                        return property.DbType.GetDescription();
                    }
                    return $"decimal({property.Precision},{property.Scale})";
                case Datatype.Child:
                case Datatype.ReadonlyChild:
                case Datatype.Reference:
                case Datatype.Long:
                case Datatype.FlagEnum:
                    return "bigint";
                case Datatype.Enum:
                    return "int";
                case Datatype.Guid:
                case Datatype.ReadOnlyGuid:
                    return "uniqueidentifier";
                case Datatype.Datetime:
                    return "datetime2(7)";
                case Datatype.Date:
                    return "date";
                case Datatype.Byte:
                    return "int";
                case Datatype.Time:
                    return "time";
                default:
                    throw new ArgumentException("Datatype was not recognized");
            }
        }
    }
}
