using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
	public class GetByIdListBuilder : GetByIdHelper
	{
		private IEnumerable<string> _declareChildIdListsQuery;

		private IEnumerable<string> _getChildByIdsQuery;

		private IEnumerable<string> _getChildCollectionByIdsQuery;

		public GetByIdListBuilder(Entity entity, Database database)
			: base(entity, database)
		{
		}

		protected virtual string TemplateWithUid => SqlResources.GetFullObjectByIdsWithUid;

		protected override string Template => SqlResources.GetFullObjectByIds;

		public virtual IEnumerable<string> DeclareChildIdListsQuery =>
			_declareChildIdListsQuery ?? (_declareChildIdListsQuery =
															GetQueryDeclareChildIdsByEntity(Entity));

		public virtual IEnumerable<string> GetChildByIdsQuery =>
			_getChildByIdsQuery ?? (_getChildByIdsQuery =
										ChildsAndChildReadonlyProperties(Entity).Select(GetQueryGetByIds));

		public virtual IEnumerable<string> GetChildCollectionByIdsQuery =>
			_getChildCollectionByIdsQuery ?? (_getChildCollectionByIdsQuery = Entity.Properties
												.Where(x => x.DataType.Datatype == Datatype.ChildCollection)
												.OrderBy(x => x.Name)
												.Select(x => $"EXEC {x.DataType.Type}_GetFullBy{Entity.Name}Ids @Ids"));


		public IEnumerable<Property> ChildChildReadonlyAndChildCollectionProperties(Entity entity) => ChildProperties(entity)
			.Concat(ChildReadonlyProperties(entity))
			.Concat(ChildCollectionProperties(entity)).OrderBy(p => p.Name);

		public static IEnumerable<Property> ChildsAndChildReadonlyProperties(Entity entity) => ChildProperties(entity)
			.Concat(ChildReadonlyProperties(entity))
			.OrderBy(p => p.Name);

		public static IEnumerable<Property> ChildProperties(Entity entity)
		{
			return entity.Properties.Where(x => x.DataType.Datatype == Datatype.Child).OrderBy(x => x.Name);
		}

		public static IEnumerable<Property> ChildReadonlyProperties(Entity entity)
		{
			return entity.Properties.Where(x => x.DataType.Datatype == Datatype.ReadonlyChild).OrderBy(x => x.Name);
		}

		public IEnumerable<Property> ChildCollectionProperties(Entity entity)
		{
			return entity.Properties.Where(x => x.DataType.Datatype == Datatype.ChildCollection).OrderBy(x => x.Name);
		}

		public override TemplateParameter GetParameters()
		{
			var parameters = base.GetParameters();
			parameters.Add(SqlConstants.DeclareChildIdLists, DeclareChildIdListsQuery);
			parameters.Add(SqlConstants.Childs_GetByIds, GetChildByIdsQuery);
			parameters.Add(SqlConstants.ChildCollection_GetByIds, GetChildCollectionByIdsQuery);

			return parameters;
		}

		public string Build()
		{
			var template = new Template();
			if (Entity.GenerateReadonlyGuid)
			{
				template.SetTemplate(TemplateWithUid);
			}
			else
			{
				template.SetTemplate(Template);
			}
			template.Fill(GetParameters());
			return template.CleanText;
		}


		protected static IEnumerable<string> GetQueryDeclareChildIdsByEntity(Entity entity)
		{
			return ChildsAndChildReadonlyProperties(entity).Select(property => GetQueryDeclareChildIds(property, entity));
		}

		protected static string GetQueryDeclareChildIds(Property property, Entity entity)
		{
			StringBuilder stringbuilder = new StringBuilder();
			stringbuilder.AppendLine($"DECLARE @{property.Name}Ids AS [dbo].BigintType;");
			stringbuilder.AppendLine($"INSERT INTO @{property.Name}Ids");
			stringbuilder.AppendLine($"SELECT {property.Name}Id");
			stringbuilder.AppendLine($"  FROM [dbo].{entity.Name}");
			stringbuilder.AppendLine("where EXISTS(SELECT 1						 ");
			stringbuilder.AppendLine("	  FROM @Ids									 ");
			stringbuilder.AppendLine($"	  WHERE[@Ids].Id = {entity.Name}.Id");
			stringbuilder.AppendLine("      )");
			stringbuilder.AppendLine($"AND {property.Name}Id IS NOT NULL;");
			return stringbuilder.ToString();
		}

		protected string GetQueryDeclareChildCollectionIds(Property property)
		{
			StringBuilder stringbuilder = new StringBuilder();
			stringbuilder.AppendLine($"DECLARE @{property.Name}Ids AS [dbo].BigintType;");
			stringbuilder.AppendLine($"INSERT INTO @{property.Name}Ids");
			stringbuilder.AppendLine($"SELECT {property.DataType.Type}Id");
			stringbuilder.AppendLine($"  FROM [dbo].{property.DataType.Type}");
			stringbuilder.AppendLine("where EXISTS(SELECT 1						 ");
			stringbuilder.AppendLine("	  FROM @Ids									 ");
			stringbuilder.AppendLine($"	  WHERE[@Ids].Id = {Entity.Name}.Id");
			stringbuilder.AppendLine("      );");

			return stringbuilder.ToString();
		}

		protected string GetQueryGetByIds(Property property)
		{
			return $"EXEC {property.DataType.Type}_GetFullByIds @{property.Name}Ids";

		}
	}
}