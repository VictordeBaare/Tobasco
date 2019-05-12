using System.Collections.Generic;
using System.Linq;
using TobascoV2.Enums;
using TobascoV2.Extensions;

namespace TobascoV2.Builder.BusinessRules
{
    public static class BusinessRuleManager
    {
        private static IDictionary<string, List<IRule>> _rules = new Dictionary<string, List<IRule>>
        {
            { Datatype.String.GetDescription(), new List<IRule> { new SizeBusinessRule() } },
            { Datatype.Long.GetDescription(), new List<IRule> { new MinMaxBusinessRule() } },
            { Datatype.Int.GetDescription(), new List<IRule> { new MinMaxBusinessRule() } }
        };

        public static IEnumerable<IRule> GetRules(string key)
        {
            if (_rules.ContainsKey(key))
            {
                return _rules[key];
            }
            return Enumerable.Empty<IRule>();
        }

        public static void AddRuleByKey(string key, IRule rule)
        {
            if (_rules.ContainsKey(key))
            {
                _rules[key].Add(rule);
            }
            else
            {
                _rules.Add(key, new List<IRule> { rule });
            }
        }
    }
}
