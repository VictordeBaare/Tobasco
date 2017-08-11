using System;
using System.Linq;

namespace Tobasco.Templates
{
    public class Template
    {
        private string _templateText;

        public void SetTemplate(string template)
        {
            _templateText = template;
        }

        public void Fill(TemplateParameter tags)
        {
            if (string.IsNullOrEmpty(_templateText))
            {
                throw new ArgumentNullException(nameof(_templateText), @"TemplateText is null. First set the desired template with TemplateText.");
            }

            foreach (var t in tags.GetParameters)
            {
                _templateText = string.IsNullOrEmpty(t.Key) ? RemoveLineWithEmptyAttribute(_templateText, $"%{t.Key}%") : _templateText.Replace($"%{t.Key}%", t.Value);
            }
        }

        public string GetText => _templateText;

        private string RemoveLineWithEmptyAttribute(string text, string attributeName)
        {
            var lines = text.Split(new []{ Environment.NewLine }, StringSplitOptions.None).ToList();

            for (int i = lines.Count - 1; i >= 0 ; i--)
            {
                if (lines[i].Contains(attributeName))
                {
                    lines.RemoveAt(i);
                }
            }

            return string.Join(Environment.NewLine, lines);
        }
    }
}
