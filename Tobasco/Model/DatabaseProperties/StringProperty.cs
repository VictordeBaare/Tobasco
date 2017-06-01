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
                return $"nvarchar({Property.DataType.Size})";
            }
        }
    }
}
