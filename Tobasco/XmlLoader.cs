using EnvDTE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Tobasco.Generation;
using Tobasco.Manager;
using Tobasco.Model;

namespace Tobasco
{
    internal class XmlLoader
    {
        private readonly DTE dTE;

        public XmlLoader(DTE dTE)
        {
            this.dTE = dTE;
        }

        internal void Load(string path, GenerationOptions generationOptions)
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
            

            var entities = new List<LoadedEntity>();
            if (generationOptions.ForceCleanAndGenerate)
            {
                OutputPaneManager.WriteToOutputPane("All xmls will be generated again.");
            }
            else
            {
                OutputPaneManager.WriteToOutputPane("Only dirty xmls will be generated again.");
            }

            foreach (var filepath in xmls.Where(x => !x.Contains("MainInfo")))
            {
                try
                {
                    XmlDocument doc = new XmlDocument();

                    if (generationOptions.ForceCleanAndGenerate)
                    {
                        doc.Load(filepath);
                        entities.Add(new LoadedEntity { Name = doc.GetElementsByTagName("Entity")[0].Attributes["name"].Value, Path = filepath, IsChanged = true });
                    }
                    else
                    {
                        var isCheckedOut = dTE.SourceControl.IsItemCheckedOut(filepath);
                        doc.Load(filepath);
                        entities.Add(new LoadedEntity { Name = doc.GetElementsByTagName("Entity")[0].Attributes["name"].Value, Path = filepath, IsChanged = isCheckedOut });
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
