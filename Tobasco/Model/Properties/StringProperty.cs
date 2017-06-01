using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Extensions;

namespace Tobasco.Model.Properties
{
    public class StringProperty : ClassProperty
    {
        public StringProperty(Property property, EntityLocation location) : base(property, location)
        {
        }

        public override List<string> CalcRules
        {
            get
            {
                var rules = base.CalcRules;
                rules.Add(CalcBusinessRule(), rule => !string.IsNullOrEmpty(rule));
                return rules;
            }
        }

        private string CalcBusinessRule()
        {
            if (_property.DataType.Size != null && _property.DataType.Size != "max")
            {
                return $"[StringLength({_property.DataType.Size}, ErrorMessage = \"Maximum length is {_property.DataType.Size} for {_property.Name}\")]";
            }
            return string.Empty;
        }
    }
}
