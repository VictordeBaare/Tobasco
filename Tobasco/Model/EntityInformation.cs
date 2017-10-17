using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tobasco.Model
{
    [XmlRoot("EntityInformation", Namespace = "http://Tobasco/MainInfoSchema.xsd")]
    public class EntityInformation
    {
        [XmlArray("EntityLocations")]
        [XmlArrayItem("EntityLocation", typeof(EntityLocation))]
        public List<EntityLocation> EntityLocations { get; set; }

        [XmlArray("BaseNamespaces")]
        [XmlArrayItem("Namespace", typeof(ValueElement))]
        public List<ValueElement> Namespaces { get; set; }

        public Mappers Mappers { get; set; }

        public Database Database { get; set; }

        public Component Component { get; set; }

        public Repository Repository { get; set; }

        public DependencyInjection DependencyInjection { get; set; }

        public ConnectionFactory ConnectionFactory { get; set; }

        public GenericRepository GenericRepository { get; set; }
    }
}
