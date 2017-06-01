using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class BaseProperty
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("required")]
        public bool Required { get; set; }

        public DataType DataType { get; set; }
    }
}
