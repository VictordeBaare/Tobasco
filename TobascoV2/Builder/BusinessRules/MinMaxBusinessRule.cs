using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Context;

namespace TobascoV2.Builder.BusinessRules
{
    public class MinMaxBusinessRule : IRule
    {
        public string GetRule(XmlProperty xmlProperty)
        {
            var min = xmlProperty.PropertyType.Min;
            var max = xmlProperty.PropertyType.Max;
            if (min != null && max != null)
            {
                return $"[Range({min}, {max})]";
            }
            if (min != null && max == null)
            {
                switch (xmlProperty.PropertyType.Name)
                {
                    case Enums.Datatype.Int:
                        return $"[Range({min}, int.MaxValue)]";
                    case Enums.Datatype.Long:
                        return $"[Range({min}, long.MaxValue)]";
                    default:
                        throw new ArgumentException($"Business rule for {xmlProperty.Name} could not be calculated");
                }
            }
            if (min == null && max != null)
            {
                switch (xmlProperty.PropertyType.Name)
                {
                    case Enums.Datatype.Int:
                        return $"[Range(int.MinValue, {max}, ErrorMessage = @\"Maximum length is {xmlProperty.PropertyType.Size} for {xmlProperty.Name}\")]";
                    case Enums.Datatype.Long:
                        return $"[Range(long.MinValue, {max})]";
                    default:
                        throw new ArgumentException($"Business rule for {xmlProperty.Name} could not be calculated");
                }
            }
            throw new ArgumentException($"Business rule for {xmlProperty.Name} could not be calculated");
        }
    }
}
