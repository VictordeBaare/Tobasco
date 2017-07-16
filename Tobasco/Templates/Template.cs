using System;
using System.Collections.Generic;

namespace Tobasco.Templates
{
    public class Template
    {
        private string TemplateText;

        public void SetTemplate(string template)
        {
            TemplateText = template;
        }

        public void Fill(Dictionary<string, string> tags)
        {
            if (string.IsNullOrEmpty(TemplateText))
            {
                throw new ArgumentNullException("TemplateText is null. First set the desired template with TemplateText.");
            }

            foreach (var t in tags)
            {
                TemplateText = TemplateText.Replace(string.Format("%{0}%", t.Key), t.Value);
            }
        }

        public string GetText => TemplateText;
    }
}
