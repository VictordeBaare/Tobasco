using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.FileBuilder
{
    public sealed class FileProperties
    {
        public FileProperties()
        {
            TemplateParameter = new Dictionary<string, string>();
        }

        public string CustomTool { get; set; }
        public string BuildAction { get; set; }
        public Dictionary<string, string> TemplateParameter { get; set; }

        internal string BuildActionString => BuildAction;
    }
}
