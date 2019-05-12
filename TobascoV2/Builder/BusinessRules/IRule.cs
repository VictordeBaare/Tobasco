using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Context;

namespace TobascoV2.Builder.BusinessRules
{
    public interface IRule
    {
        string GetRule(XmlProperty xmlProperty);
    }
}
