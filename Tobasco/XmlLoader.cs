using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Tobasco.Manager;
using Tobasco.Model;

namespace Tobasco
{
    public class XmlLoader
    {      
        public void Load(string path)
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
            EntityManager.Initialise(entities);
        }
    }
}
