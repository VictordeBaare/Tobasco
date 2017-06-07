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
        private readonly ORMapper _orMapper;
        private readonly bool _generateRules;

        public PropertyClassFactory(ORMapper orMapper, bool generateRules)
        {
            _orMapper = orMapper;
            _generateRules = generateRules;
        }

        public ClassProperty GetProperty(Property property)
        {
            switch (property.DataType.Datatype)
            {
                case Datatype.String:
                    return new StringProperty(property, _orMapper, _generateRules);
                case Datatype.Int:
                case Datatype.Long:
                    return new NumericProperty(property, _orMapper, _generateRules);
                case Datatype.Decimal:
                    return new ClassProperty(property, _orMapper, _generateRules);
                default:
                    return new ClassProperty(property, _orMapper, _generateRules);
            }
        }
    }
}
