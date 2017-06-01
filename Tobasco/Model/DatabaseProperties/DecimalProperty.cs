using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Model.DatabaseProperties
{
    public class DecimalProperty : DatabaseProperty
    {
        public DecimalProperty(Property property) : base(property)
        {
        }

        protected override string GetValueType
        {
            get
            {
                return $"decimal({Property.DataType.Precision},{Property.DataType.Scale})";
            }
        }
    }
}
