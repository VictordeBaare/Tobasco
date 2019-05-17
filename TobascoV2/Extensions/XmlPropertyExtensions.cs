using TobascoV2.Context;

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
