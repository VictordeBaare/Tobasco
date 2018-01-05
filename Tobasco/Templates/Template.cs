using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Tobasco.Manager;

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
                _templateText = RemoveEmptyKeys(t, _templateText);
            }

            //_templateText = RemoveDoubleWhiteLines(_templateText);
        }

        public string GetText => _templateText;

        private string RemoveEmptyKeys(KeyValuePair<string, string> keyValuePair, string text)
        {
            if (string.IsNullOrEmpty(keyValuePair.Key))
            {
                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();                            
                for (int i = lines.Count - 1; i >= 0; i--)
                {                    
                    if (lines[i].Contains($"%{keyValuePair.Key}%"))
                    {
                        lines.RemoveAt(i);
                    }                    
                }
                return string.Join(Environment.NewLine, lines);
            }
            else if (string.IsNullOrEmpty(keyValuePair.Value))
            {
                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    if (lines[i].Trim().Equals($"%{keyValuePair.Key}%"))
                    {
                        lines.RemoveAt(i);
                    }
                    else
                    {
                        lines[i] = lines[i].Replace($"%{keyValuePair.Key}%", keyValuePair.Value);
                    }
                }
                return string.Join(Environment.NewLine, lines);
            }else
            {
                return _templateText.Replace($"%{keyValuePair.Key}%", keyValuePair.Value);
            }
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
