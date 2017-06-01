using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class FromTo
    {
        [XmlAttribute("from")]
        public string From { get; set; }

        [XmlAttribute("to")]
        public string To { get; set; }
    }
}
