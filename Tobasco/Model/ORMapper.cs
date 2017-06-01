using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tobasco.Enums;

namespace Tobasco.Model
{
    public class ORMapper
    {
        [XmlAttribute("type")]
        public OrmType Type { get; set; }        
    }
}
