using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.RepositoryBuilders
{
    public class GetFullEntityByIdReader : GetFullEntityByIdHelper
    {
        public GetFullEntityByIdReader(EntityHandler entity, Repository repository) : base(entity, repository)
        {
        }

        public string Build()
        {
            var template = new Template();
            if(GetChildProperties.Any())
            {
                template.SetTemplate(RepositoryResources.RepositoryGetFullByIdReader);
                template.Fill(GetParatmers());
            }
            else if (GetChildCollectionProperties.Any())
            {
                template.SetTemplate(RepositoryResources.RepositoryGetFullyByIdChildCollectionReader);
                template.Fill(GetChildCollectionReaderParameters());
            }
            else
            {
                template.SetTemplate(RepositoryResources.RepositoryGetFullByIdReaderWithoutProp);
                template.Fill(base.GetParatmers());
            }
            return template.GetText;
        }

        private TemplateParameter GetChildCollectionReaderParameters()
        {
            var parameters = base.GetParatmers();
            parameters.Add(RepositoryBuilderConstants.ChildReadDictionary, GetChildReadAction());
            parameters.Add(RepositoryBuilderConstants.ChildReader, GetChildReaders());
            parameters.Add(RepositoryBuilderConstants.ChildCollectionReader, GetChildCollectionReaders());
            parameters.Add(RepositoryBuilderConstants.ChildCollectionReadDictionary, GetChildCollectionReadAction("returnItem"));
            return parameters;
        }

        protected override TemplateParameter GetParatmers()
        {
            var parameters = base.GetParatmers();
            parameters.Add(RepositoryBuilderConstants.ChildCollectionReader, GetChildCollectionReaders());
            parameters.Add(RepositoryBuilderConstants.ChildReader, GetChildReaders());
            parameters.Add(RepositoryBuilderConstants.ChildCollectionReadDictionary, GetChildCollectionReadAction("item"));
            parameters.Add(RepositoryBuilderConstants.ChildReadDictionary, GetChildReadAction());
            parameters.Add(RepositoryBuilderConstants.SplitOn, GetSplitOn());
            parameters.Add(RepositoryBuilderConstants.ReaderParameters, GetParameterReader());
            return parameters;                
        }

        private IEnumerable<string> GetChildCollectionReaders()
        {
            var list = new List<string>();

            foreach(var prop in GetChildCollectionProperties)
            {
                list.Add($"var {prop.Name}Dict = {Entity.GetRepositoryOnName(prop.DataType.Type)}.Read(reader);");
            }

            return list;
        }

        private IEnumerable<string> GetChildReaders()
        {
            var list = new List<string>();

            foreach (var prop in GetChildProperties)
            {
                list.Add($"var {prop.Name}Dict = {Entity.GetRepositoryOnName(prop.DataType.Type)}.Read(reader);");
            }

            foreach (var prop in GetChildReadonlyProperties)
            {
                list.Add($"var {prop.Name}Dict = {Entity.GetRepositoryOnName(prop.DataType.Type)}.Read(reader);");
            }

            return list;
        }

        private IEnumerable<string> GetChildReadAction()
        {
            var list = new List<string>();

            foreach (var prop in GetChildProperties)
            {
                var builder = new StringBuilder();
                builder.AppendLine($"if ({prop.Name}Dict.ContainsKey({prop.Name}))");
                builder.AppendLine("{");
                builder.AppendLine($"item.{prop.Name} = {prop.Name}Dict[{prop.Name}];");
                builder.AppendLine("}");
                list.Add(builder.ToString());
            }

            foreach (var prop in GetChildReadonlyProperties)
            {
                var builder = new StringBuilder();
                builder.AppendLine($"if ({prop.Name}Dict.ContainsKey({prop.Name}))");
                builder.AppendLine("{");
                builder.AppendLine($"item.{prop.Name} = {prop.Name}Dict[{prop.Name}];");
                builder.AppendLine("}");
                list.Add(builder.ToString());
            }

            return list;
        }

        private IEnumerable<string> GetChildCollectionReadAction(string itemName)
        {
            var list = new List<string>();

            foreach (var prop in GetChildCollectionProperties)
            {
                var builder = new StringBuilder();
                builder.AppendLine($"foreach (var obj in {prop.Name}Dict.Values.Where(x => x.{Entity.GetChildReferenceProperty(prop.DataType.Type, Entity.Entity.Name).Name} == {itemName}.Id))");
                builder.AppendLine("{");
                builder.AppendLine($"{itemName}.{prop.Name}.Add(obj);");
                builder.AppendLine("}");
                list.Add(builder.ToString());
            }

            return list;
        }

        private string GetSplitOn()
        {
            var list = new List<string>();

            foreach(var prop in GetChildProperties)
            {
                list.Add(prop.Name);
            }

            foreach(var prop in GetChildReadonlyProperties)
            {
                list.Add(prop.Name);
            }

            return string.Join(",", list);
        }

        private string GetParameterReader()
        {
            var list = new List<string>();

            list.Add($"{Entity.Entity.Name} item");

            foreach (var prop in GetChildProperties)
            {
                list.Add($"long {prop.Name}");
            }

            foreach (var prop in GetChildReadonlyProperties)
            {
                list.Add($"long {prop.Name}");
            }

            return string.Join(",", list);
        }
    }
}
