using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class DescriptionBuilder : DatabaseHelper
    {
        public DescriptionBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public virtual string GetTemplateTableDescription => SqlResources.DescriptionTable;

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(GetTemplateTableDescription);
            template.Fill(GetParameters());
            return template.GetText;
        }

        private TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.Description, string.IsNullOrEmpty(Entity.Description) ? Name : Entity.Description);
            parameters.Add(SqlConstants.DescriptionColumns, GetColumnDescriptions());
            return parameters;
        }

        private List<string> GetColumnDescriptions()
        {
            var columnDescriptions = new List<string>();
            foreach(var prop in Entity.GetSqlProperties.Where(x => x.Property.DataType.Datatype != Enums.Datatype.ChildCollection))
            {                
                var builder = new DescriptionColumnBuilder(prop, Entity);
                if (builder.HasValue)
                {
                    columnDescriptions.Add(builder.Build());
                }
            }
            return columnDescriptions;
        }
    }
}
