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

        public ValueElement NamespaceOverride { get; set; }

        [XmlIgnore]
        public string GetNamespace => _getNamespace ?? (_getNamespace = $"namespace {GetProjectLocation}");

        [XmlIgnore]
        public string GetProjectLocation
        {
            get
            {
                if (!string.IsNullOrEmpty(NamespaceOverride?.Value))
                {
                    return NamespaceOverride.Value;
                }
                return _getProjectLocation ?? (_getProjectLocation = $"{(AssemblyName != null ? AssemblyName.Value + "." : string.Empty)}{Project}{(!string.IsNullOrEmpty(Folder) ? $".{Folder}" : string.Empty)}");
            }

        }
    }
}
