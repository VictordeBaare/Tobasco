﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TobascoV2.Enums;

namespace TobascoV2.Context
{
    public class XmlPropertyType
    {
        [XmlAttribute("name")]
        public Datatype Name { get; set; }

        [XmlAttribute("dbtype")]
        public DataDbType DbType { get; set; }

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

        [XmlAttribute("ignoreforeignkey")]
        public bool IgnoreForeignKey { get; set; }

        [XmlAttribute("ignoreforeignkeyindex")]
        public bool IgnoreForeignKeyIndex { get; set; }

    }
}
