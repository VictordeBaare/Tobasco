using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Tobasco.Manager;
using Tobasco.Model;

namespace Tobasco
{
    internal class XmlLoader
    {      
        internal void Load(string path)
        {            
            XmlSerializer entityserializer = new XmlSerializer(typeof(Entity));
            XmlSerializer serializer = new XmlSerializer(typeof(EntityInformation));

            var entityDictionary = new Dictionary<string, EntityHandler>();
            var xmls = Directory.GetFiles(path, "*.xml");
            try
            {
                using (var reader = new StreamReader(Path.Combine(path, "MainInfo.xml")))
                {
                    MainInfoManager.Initialize((EntityInformation)serializer.Deserialize(reader));
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error with reading maininfo xml: {ex}");
            }
            

            var entities = new List<NameWithPath>();
            foreach (var filepath in xmls.Where(x => !x.Contains("MainInfo")))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filepath);
                    entities.Add(new NameWithPath { Name = doc.GetElementsByTagName("Entity")[0].Attributes["name"].Value, Path = filepath });
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error with reading xml: {filepath}", ex);
                }
            }
            EntityManager.Initialise(entities);
        }
    }
}
