using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Tobasco.Factories;
using Tobasco.Model.DatabaseProperties;

namespace Tobasco.Model
{
    [XmlRoot("Entity", Namespace = "http://Tobasco/EntitySchema.xsd")]
    public class Entity
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("isabstract")]
        public bool IsAbstract { get; set; }

        [XmlArray("Properties")]
        [XmlArrayItem("Property", typeof(Property))]
        public List<Property> Properties { get; set; }

        [XmlAttribute("generateReadonlyGuid")]
        public bool GenerateReadonlyGuid { get; set; }

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


        private IEnumerable<DatabaseProperty> _getSqlProperties;
        [XmlIgnore]
        public virtual IEnumerable<DatabaseProperty> GetSqlProperties => _getSqlProperties ?? (_getSqlProperties = Properties.Select(x => DatabasePropertyFactory.GetDatabaseProperty(x)));

    }
}
