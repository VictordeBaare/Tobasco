using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
	public class GetByReferenceIdListBuilder : GetByIdHelper
	{
		private Property _reference;

		public GetByReferenceIdListBuilder(Entity entity, Database database) : base(entity, database)
		{
		}

		public string Build(Property reference)
		{
			_reference = reference;
			var template = new Template();
			template.SetTemplate(Template);
			template.Fill(GetReferenceParameters(reference));
			return template.GetText;
		}

		private TemplateParameter GetReferenceParameters(Property reference)
		{
			var parameters = GetParameters();
			parameters.Add(SqlConstants.ReferenceName, reference.Name);
			return parameters;
		}

		public override IEnumerable<string> GetChildCollectionGetByParentId
		{
			get
			{
				return _getChildCollectionByParentId ?? (_getChildCollectionByParentId = Entity.Properties
					.Where(x => x.DataType.Datatype == Datatype.ChildCollection)
					.Select(x => $"EXEC {x.DataType.Type}_GetFullBy{Entity.Name}Ids @{_reference.Name}"));
			}
		}

		protected override string Template => SqlResources.SqlGetByReferenceIdList;
	}
}
