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
        public StringProperty(Property property, ORMapper mapper, bool generateRules) : base(property, mapper, generateRules)
        {
        }

        public override StringBuilder CalcRules
        {
            get
            {
                var builder = base.CalcRules;
                if (_generateRules)
                {
                    var rule = CalcBusinessRule();
                    if (!string.IsNullOrEmpty(rule))
                    {
                        builder.AppendLine(rule);
                    }                    
                }
                return builder;
            }
        }

        private string CalcBusinessRule()
        {
            if (Property.DataType.Size != null && Property.DataType.Size != "max")
            {
                return $"[StringLength({Property.DataType.Size}, ErrorMessage = \"Maximum length is {Property.DataType.Size} for {Property.Name}\")]";
            }
            return string.Empty;
        }
    }
}
