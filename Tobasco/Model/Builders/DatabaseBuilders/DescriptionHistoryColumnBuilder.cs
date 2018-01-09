using Tobasco.Constants;
using Tobasco.Model.DatabaseProperties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class DescriptionHistoryColumnBuilder : DescriptionColumnBuilder
    {
        public DescriptionHistoryColumnBuilder(DatabaseProperty property, Entity entity, EntityInformation information) : base(property, entity, information)
        {
        }

        protected override TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, $"{_entity.Name}_historie");
            parameters.Add(SqlConstants.Description, GetColumnDescription());
            parameters.Add(SqlConstants.Columnname, _property.SelectSqlParameterNaam);
            return parameters;
        }
    }
}
