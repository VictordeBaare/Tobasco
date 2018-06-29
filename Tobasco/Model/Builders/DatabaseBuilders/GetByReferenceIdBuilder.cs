using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class GetByReferenceIdBuilder : GetByIdHelper
    {
        private Property _reference;

        public GetByReferenceIdBuilder(Entity entity, Database database) : base(entity, database)
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

        protected override IEnumerable<string> GetChildCollectionGetByParentId
        {
            get
            {
                return _getChildCollectionByParentId ?? (_getChildCollectionByParentId = Entity.Properties
                    .Where(x => x.DataType.Datatype == Datatype.ChildCollection)
                    .Select(x => $"EXEC {x.DataType.Type}_GetFullBy{Entity.Name}Id @{_reference.Name}"));
            }
        }

        protected override IEnumerable<string> DeclareChilds
        {
            get
            {
                return _getDeclareChilds ?? (_getDeclareChilds = Entity.GetSqlProperties
                    .Where(x => x.Property.DataType.Datatype == Datatype.ReadonlyChild || x.Property.DataType.Datatype == Datatype.Child)
                    .Select(x => GetChildDeclareId(x, $"@{_reference.Name}")));
            }
        }

        protected override string Template => SqlResources.SqlGetByReferenceId;
    }
}
