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
        private readonly DynamicTextTransformation2 _textTransformation;
        
        public XmlLoader(DynamicTextTransformation2 textTransformation)
        {
            if (textTransformation != null)
            {
                _textTransformation = textTransformation;
            }
        }

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
                using (var reader = new StreamReader(filepath))
                {
                    entities.Add((Entity)entityserializer.Deserialize(reader));
                }
            }


            return new MainHandler(mainInformation, entities);
        }

        public void WriteComment(string comment)
        {
            if (_textTransformation != null)
            {
                _textTransformation.WriteLine(comment.StartsWith("//") ? comment : "//" + comment);
            }
        }
    }
}
