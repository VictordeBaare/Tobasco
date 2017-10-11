using System.Xml.Serialization;

namespace Tobasco.Model
{
    public class StoredProcedures : FolderWithGenerate
    {
        [XmlElement("Insert")]
        public GenerateElement GenerateInsert { get; set; }

        [XmlElement("Update")]
        public GenerateElement GenerateUpdate { get; set; }

        [XmlElement("Delete")]
        public GenerateElement GenerateDelete { get; set; }

        [XmlElement("GetById")]
        public GenerateElement GenerateGetById { get; set; }

        [XmlElement("Merge")]
        public GenerateElement GenerateMerge { get; set; }
    }
}