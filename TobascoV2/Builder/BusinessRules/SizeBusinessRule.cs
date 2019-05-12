using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Context;

namespace TobascoV2.Builder.BusinessRules
{
    public class SizeBusinessRule : IRule
    {
        public string GetRule(XmlProperty xmlProperty)
        {
            return $"[StringLength({xmlProperty.PropertyType.Size}, ErrorMessage = @\"Maximum length is {xmlProperty.PropertyType.Size} for {xmlProperty.Name}\")]";
        }
    }
}
