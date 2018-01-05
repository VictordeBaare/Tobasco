using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Constants;
using Tobasco.Model.DatabaseProperties;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class DescriptionColumnBuilder
    {
        private DatabaseProperty _property;
        private Entity _entity;

        public DescriptionColumnBuilder(DatabaseProperty property, Entity entity)
        {
            _property = property;
            _entity = entity;
        }

        public virtual string GetTemplateColumnDescription => SqlResources.DescriptionColumn;

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(GetTemplateColumnDescription);
            template.Fill(GetParameters());
            return template.GetText;
        }

        public bool HasValue => !string.IsNullOrEmpty(_property.Property.Description);

        private TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, _entity.Name);
            parameters.Add(SqlConstants.Description, _property.Property.Description);
            parameters.Add(SqlConstants.Columnname, _property.SelectSqlParameterNaam);
            return parameters;
        }

    }
}
