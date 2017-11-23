using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tobasco;
using Tobasco.Model;

namespace Tobasco
{
    public class XmlLoader
    {      
        public MainHandler Load(string path)
        {            
            XmlSerializer entityserializer = new XmlSerializer(typeof(Entity));
            XmlSerializer serializer = new XmlSerializer(typeof(EntityInformation));

            var entityDictionary = new Dictionary<string, EntityHandler>();
            EntityInformation mainInformation;
            var xmls = Directory.GetFiles(path, "*.xml");
            using (var reader = new StreamReader(Path.Combine(path, "MainInfo.xml")))
            {
                mainInformation = (EntityInformation)serializer.Deserialize(reader);
            }

            var entities = new List<Entity>();

            foreach (var filepath in xmls.Where(x => !x.Contains("MainInfo")))
            {
                try
                {
                    using (var reader = new StreamReader(filepath))
                    {
                        entities.Add((Entity)entityserializer.Deserialize(reader));
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error with reading xml: {filepath}", ex);
                }                
            }

            return new MainHandler(mainInformation, entities);
        }
    }
}
