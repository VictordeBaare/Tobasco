using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

            _templateText = RemoveDoubleWhiteLines(_templateText);
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

        public string CleanText => RemoveDoubleWhiteLines(_templateText);

        private string RemoveDoubleWhiteLines(string text)
        {
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            var toRemoveLines = new List<int>();
            for (int i = 0; i < lines.Count; i++)
            {
                if (Regex.IsMatch(lines[i], "^[\t]*$") && ((i + 1) < lines.Count && (lines[i + 1] == "}") || Regex.IsMatch(lines[i + 1], "^[\t]*$")))
                {
                    if (!toRemoveLines.Contains(i))
                    {
                        toRemoveLines.Add(i);
                    }
                }

                if (lines[i] == "{" && (i + 1) < lines.Count && Regex.IsMatch(lines[i + 1], "^[\t]*$"))
                {
                    if (!toRemoveLines.Contains(i + 1 ))
                    {
                        toRemoveLines.Add(i + 1);
                    }
                }
            }

            foreach(var index in toRemoveLines.OrderByDescending(x => x))
            {
                lines.RemoveAt(index);
            }

            return string.Join(Environment.NewLine, lines);
        }
    }
}
