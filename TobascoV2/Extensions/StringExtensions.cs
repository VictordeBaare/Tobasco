using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobascoV2.Extensions
{
    public static class StringExtensions
    {
        public static string GetObjectWithFieldName(this string value)
        {
            return $"{value} {GetFieldName(value)}";
        }

        public static string GetObjectWithParamaterName(this string value)
        {
            return $"{value} {GetParameterName(value)}";
        }

        public static string GetFieldName(this string value)
        {
            return $"_{value.ToLower()}";
        }

        public static string GetParameterName(this string value)
        {
            return value.ToLower();
        }
    }
}
