using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Tobasco.Enums;

namespace Tobasco.Model
{
    public class DataType
    {
        [XmlAttribute("name")]
        public Datatype Datatype { get; set; }

        [XmlAttribute("size")]
        public string Size { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("min")]
        public string Min { get; set; }

        [XmlAttribute("max")]
        public string Max { get; set; }

        [XmlAttribute("precision")]
        public string Precision { get; set; }

        [XmlAttribute("scale")]
        public string Scale { get; set; }

        [XmlAttribute("donotgenerateforeignkey")]
        public bool DoNotGenerateForeignKey { get; set; }
    }
}
