using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class ValueElement
    {
        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
