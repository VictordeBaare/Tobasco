using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;
using Tobasco.Factories;

namespace Tobasco.Model.Properties
{
    public class ClassProperty
    {
        protected Property _property;
        private EntityLocation _location;
        private string _getProperty;
        private string _getValueType;

        public ClassProperty(Property property, EntityLocation location)
        {
            _property = property;
            _location = location;
        }

        public Property Property
        {
            get { return _property; }
        }

        public string GetProperty
        {
            get
            {
                if (string.IsNullOrEmpty(_getProperty))
                {
                    _getProperty = OrmPropertyFactory.GeefPropertyBuilder(_location.ORMapper != null ? _location.ORMapper.Type : OrmType.Onbekend).GetProperty(this, _location.GenerateRules);
                }
                return _getProperty;
            }            
        }

        public virtual List<string> CalcRules
        {
            get
            {
                var rules = new List<string>();
                var required = GetRequiredRule();
                if (!string.IsNullOrEmpty(required))
                {
                    rules.Add(required);
                }
                return rules;
            }
        }

        private string GetRequiredRule()
        {
            if (Property.Required)
            {
                return $"[Required(ErrorMessage=\"{Property.Name} is required\")]";
            }
            return string.Empty;
        }

        public virtual string GetValueType
        {
            get
            {
                if (string.IsNullOrEmpty(_getValueType))
                {
                    var isRequired = _property.Required ? string.Empty : "?" ;

                    switch (_property.DataType.Datatype)
                    {
                        case Datatype.Boolean:
                            _getValueType = $"bool{isRequired}";
                            break;
                        case Datatype.String:
                            _getValueType = "string";
                            break;
                        case Datatype.Int:
                            _getValueType = $"int{isRequired}";
                            break;
                        case Datatype.Decimal:
                            _getValueType = $"decimal{isRequired}";
                            break;
                        case Datatype.Datetime:
                        case Datatype.Date:
                            _getValueType = $"DateTime{isRequired}";
                            break;
                        case Datatype.Byte:
                            _getValueType = $"byte{isRequired}";
                            break;
                        case Datatype.ByteArray:
                            _getValueType = "byte[]";
                            break;
                        case Datatype.Reference:
                        case Datatype.Long:
                            _getValueType = $"long{isRequired}";
                            break;
                        case Datatype.Guid:
                            _getValueType = $"Guid";
                            break;
                        case Datatype.Child:
                        case Datatype.CustomType:
                        case Datatype.ChildCollection:
                            _getValueType = _property.DataType.Type;
                            break;
                        case Datatype.Enum:
                        case Datatype.FlagEnum:
                            _getValueType = $"{_property.DataType.Type}{isRequired}";
                            break;
                        default:
                            throw new Exception("Value type kon niet bepaald worden.");
                    }
                }

                return _getValueType;
            }
        }

        
    }
}
