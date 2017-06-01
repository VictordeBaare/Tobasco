using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Model;
using Tobasco.Enums;
using Tobasco.Model.Properties;

namespace Tobasco.Factories
{
    public class PropertyClassFactory
    {
        private EntityLocation _location;

        public PropertyClassFactory(EntityLocation location)
        {
            _location = location;
        }

        public ClassProperty GetProperty(Property property)
        {
            switch (property.DataType.Datatype)
            {
                case Datatype.String:
                    return new StringProperty(property, _location);
                case Datatype.Int:
                case Datatype.Long:
                    return new NumericProperty(property, _location);
                case Datatype.Decimal:
                    return new ClassProperty(property, _location);
                default:
                    return new ClassProperty(property, _location);
            }
        }
    }
}
