using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class FileLocation
    {
        private string _getNamespace;
        private string _getProjectLocation;

        [XmlAttribute("project")]
        public string Project { get; set; }

        [XmlAttribute("folder")]
        public string Folder { get; set; }

        public ValueElement AssemblyName { get; set; }

        [XmlIgnore]
        public string GetNamespace => _getNamespace ?? (_getNamespace = $"namespace {(AssemblyName != null ? AssemblyName.Value + "." : string.Empty)}{GetProjectLocation}");

        [XmlIgnore]
        public string GetProjectLocation => _getProjectLocation ?? (_getProjectLocation = $"{Project}.{Folder}");
    }
}
