using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;
using Tobasco.Extensions;

namespace Tobasco.FileBuilder
{
    public abstract class OutputFile
    {
        public abstract FileType Type { get; }

        public string NameExtension { get; set; }

        public List<string> Namespaces { get; set; } = new List<string> { "System"};

        public string OwnNamespace { get; set; }

        public List<string> Properties { get; set; } = new List<string>();

        public List<string> Fields { get; set; } = new List<string>();

        public List<string> Methods { get; set; } = new List<string>();

        public string Name { get; set; }
        public string FileName { get; set; }

        public string BaseClass { get; set; }

        public string FolderName { get; set; }

        public string ProjectName { get; set; }

        public abstract string BuildContent();

        public FileProperties FileProperties { get; set; }

        public List<string> ClassAttributes { get; set; } = new List<string>();
    }
}
