﻿using System.Xml.Serialization;
using Tobasco.Enums;

namespace Tobasco.Model
{
    public class Module
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public ModuleType Type { get; set; }

        [XmlElement("Filelocation")]
        public FileLocation FileLocation { get; set; }
    }
}