using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Extensions;

namespace Tobasco.Model.Properties
{
    public class NumericProperty : ClassProperty
    {
        public NumericProperty(Property property, ORMapper ormapper, bool generateRules) : base(property, ormapper, generateRules)
        {
        }

        public override List<string> CalcRules
        {
            get
            {
                var rules = base.CalcRules;
                rules.Add(CalcRangeBusinessRule(), rule => !string.IsNullOrEmpty(rule));
                return rules;
            }
        }

        private string CalcRangeBusinessRule()
        {
            var min = Property.DataType.Min;
            var max = Property.DataType.Max;
            if (min != null && max != null)
            {
                return $"[Range({min}, {max})]";
            }
            if (min != null && max == null)
            {
                switch (Property.DataType.Datatype)
                {
                    case Enums.Datatype.Int:
                        return $"[Range({min}, int.MaxValue)]";
                    case Enums.Datatype.Long:
                        return $"[Range({min}, long.MaxValue)]";
                    default:
                        throw new ArgumentException($"Business rule for {Property.DataType.Datatype} could not be calculated");
                }
            }
            if (min == null && max != null)
            {
                switch (Property.DataType.Datatype)
                {
                    case Enums.Datatype.Int:
                        return $"[Range(int.MinValue, {max})]";
                    case Enums.Datatype.Long:
                        return $"[Range(long.MinValue, {max})]";
                    default:
                        throw new ArgumentException($"Business rule for {Property.DataType.Datatype} could not be calculated");
                }
            }
            return string.Empty;
        }
    }
}
