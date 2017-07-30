using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Templates
{
    public class TemplateParameter 
    {
        private Dictionary<string, string> _parameters { get; } = new Dictionary<string, string>();


        public void Add(string key, string value)
        {
            _parameters.Add(key, value);
        }

        public void Add(string key, IEnumerable<string> values)
        {
            _parameters.Add(key, string.Join(Environment.NewLine, values));
        }

        public Dictionary<string, string> GetParameters => _parameters;
    }
}
