using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Model.DatabaseProperties
{
    public class StringProperty : DatabaseProperty
    {
        public StringProperty(Property property) : base(property)
        {
        }

        protected override string GetValueType
        {
            get
            {
                if (Property.DataType.DbType == Enums.DataDbType.Varchar || Property.DataType.DbType == Enums.DataDbType.Nvarchar)
                {
                    return $"{GetDbValueType}({Property.DataType.Size})";
                }
                return $"nvarchar({Property.DataType.Size})";
            }
        }
    }
}
