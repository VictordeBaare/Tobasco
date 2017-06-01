using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class FolderWithGenerate
    {
        [XmlAttribute("generate")]
        public bool Generate { get; set; }

        [XmlAttribute("folder")]
        public string Folder { get; set; }
    }
}
