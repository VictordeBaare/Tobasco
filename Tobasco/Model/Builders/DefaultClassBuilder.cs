using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tobasco.Enums;
using Tobasco.FileBuilder;
using Tobasco.Manager;
using Tobasco.Model.Builders.Base;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders
{
	public class DefaultClassBuilder : ClassBuilderBase
	{

		public DefaultClassBuilder(Entity entity, EntityLocation location) : base(entity, location)
		{
		}


		private string GenerateMethods()
		{
			var template = new Template();
			var orm = Location.ORMapper?.Type ?? OrmType.Onbekend;

			switch (orm)
			{
				case OrmType.Dapper:
					template.SetTemplate(Resources.ClassDapperMethod);
					template.Fill(GetParameters());
					return template.GetText;
				default:
					return string.Empty;
			}
		}

		protected virtual TemplateParameter GetParameters()
		{
			var list = GetAnymonousPropertySet();

			var parameters = new TemplateParameter();
			parameters.Add(Resources.AnymonousPropertySet, list);
			parameters.Add("GetChildrenOverrideMethod", GetChildrenOverrideMethodBody());
			return parameters;
		}

		protected virtual List<string> GetAnymonousPropertySet()
		{
			var list = new List<string>();
			foreach (var property in GetProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection))
			{
				if (property.Property.DataType.Datatype == Datatype.Child || property.Property.DataType.Datatype == Datatype.ReadonlyChild)
				{
					if (property.Property.Required)
					{
						list.Add($"anymonous.{property.Property.Name}Id = {property.Property.Name}.Id;");
					}
					else
					{
						list.Add($"anymonous.{property.Property.Name}Id = {property.Property.Name}?.Id;");
					}
				}
				else
				{
					list.Add($"anymonous.{property.Property.Name} = {property.Property.Name};");
				}
			}

			return list;
		}

		private string GetChildrenOverrideMethodBody()
		{

			var sb = new StringBuilder();
			sb.AppendLine(@"public override IEnumerable<EntityBase> GetChildren()");
			sb.AppendLine(@"{");

			sb.AppendLine("foreach (var item in base.GetChildren())");
			sb.AppendLine("{									   ");
			sb.AppendLine("	yield return item;					   ");
			sb.AppendLine("}									   ");

			var childrenNotCollections = GetChildNotCollectionProperties;
			var childCollections = GetChildCollectionProperties;
			if (childCollections.Any() || childrenNotCollections.Any())
			{

				foreach (var childCollection in childCollections)
				{
					sb.AppendLine($"if({childCollection.Property.Name} !=null)");
					sb.AppendLine(@"{");
					sb.AppendLine($"foreach (var item in {childCollection.Property.Name})");
					sb.AppendLine(@"{");
					sb.AppendLine("yield return item;");
					sb.AppendLine(@"}");
					sb.AppendLine(@"}");
				}

				foreach (var childNotCollection in childrenNotCollections)
				{
					sb.AppendLine($"if({childNotCollection.Property.Name} !=null)");
					sb.AppendLine(@"{");
					sb.AppendLine($"yield return {childNotCollection.Property.Name};");
					sb.AppendLine(@"}");
				}

			}
			sb.AppendLine(@"}");
			return sb.ToString();
		}

		public override IEnumerable<FileBuilder.OutputFile> Build()
		{
			OutputPaneManager.WriteToOutputPane($"Build {Entity.Name} for {Location.FileLocation.GetProjectLocation}");
			var file = FileManager.StartNewClassFile(Entity.Name, Location.FileLocation.Project, Location.FileLocation.Folder);
			file.ClassAttributes.Add("[Serializable]");
			file.IsAbstract = Entity.IsAbstract;
			file.BaseClass = Location.GetBaseClass;
			file.Namespaces.AddRange(Location.Namespaces.Select(x => x.Value).Concat(MainInfoManager.GetBasicNamespaces));
			file.Namespaces.Add(MainInfoManager.GetEnumNamespace);
			file.Properties.AddRange(GetProperties.Select(x => x.GetProperty));
			file.Methods.Add(GenerateMethods());
			file.OwnNamespace = Location.FileLocation.GetNamespace;
			return new List<FileBuilder.OutputFile> { file };
		}
	}
}
