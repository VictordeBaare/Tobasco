using System;
using TobascoV2.Context;
using TobascoV2.Enums;

namespace TobascoV2.Extensions
{
    public static class XmlPropertyExtensions
    {
        public static string GetFieldName(this XmlProperty property)
        {
            return $"_{property.Name.ToLower()}";
        }        
    }
}
