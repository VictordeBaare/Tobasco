using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class Database
    {
        [XmlAttribute("project")]
        public string Project { get; set; }

        public StoredProcedures StoredProcedures { get; set; }

        public Tables Tables { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }
    }
}
