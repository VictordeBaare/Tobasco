using System;
using System.Collections.Generic;
using System.Linq;

namespace Tobasco.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToLower(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(value, "value cannot be null");
            }

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string Join(this IEnumerable<string> values, string split, string lastSplit)
        {
            return string.Join(", ", values.ToArray(), 0, values.Count() - 1) + lastSplit + values.LastOrDefault();
        }
            
        public static string Join(this IEnumerable<string> values, string split)
        {
            return string.Join(", ", values.ToArray());
        }
    }
}
