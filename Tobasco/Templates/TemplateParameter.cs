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
            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = value;
            }
            else
            {
                _parameters.Add(key, value);
            }
        }

        public void Add(string key, IEnumerable<string> values)
        {
            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = string.Join(Environment.NewLine, values);
            }
            else
            {
                _parameters.Add(key, string.Join(Environment.NewLine, values));
            }            
        }

        public Dictionary<string, string> GetParameters => _parameters;
    }
}
