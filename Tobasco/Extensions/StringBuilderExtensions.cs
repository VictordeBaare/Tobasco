using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void AppendWithTabs(this StringBuilder builder, string text, int amountOfTabs)
        {
            string tabsText = string.Empty;
            for (int i = 0; i < amountOfTabs; i++)
            {
                tabsText += "    ";
            }
            builder.Append(tabsText + text);
        }

        public static void AppendLineWithTabs(this StringBuilder builder, string text, int amountOfTabs)
        {
            string tabsText = string.Empty;
            for (int i = 0; i < amountOfTabs; i++)
            {
                tabsText += "    ";
            }
            builder.AppendLine(tabsText + text);
        }
    }
}
