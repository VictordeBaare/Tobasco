using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Manager;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class DescriptionBuilder : DatabaseHelper
    {
        public DescriptionBuilder(Entity entity, Database database, EntityInformation information) : base(entity, database, information)
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

        public string BuildHistorie()
        {
            var template = new Template();
            template.SetTemplate(GetTemplateTableDescription);
            template.Fill(GetHistoryParameters());
            return template.GetText;
        }

        private TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.Description, string.IsNullOrEmpty(Entity.Description) ? Name : Entity.Description);
            parameters.Add(SqlConstants.DescriptionColumns, GetColumnDescriptions);
            return parameters;
        }

        private TemplateParameter GetHistoryParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, $"{Name}_historie");
            parameters.Add(SqlConstants.Description, string.IsNullOrEmpty(Entity.Description) ? Name : Entity.Description);
            parameters.Add(SqlConstants.DescriptionColumns, GetHistoryColumnDescriptions);
            return parameters;
        }

        private List<string> _getColumnDescriptions;
        private List<string> GetColumnDescriptions => _getColumnDescriptions ?? (_getColumnDescriptions = ResolveColumns());
        private List<string> _getHistoryColumnDescriptions;
        private List<string> GetHistoryColumnDescriptions => _getHistoryColumnDescriptions ?? (_getHistoryColumnDescriptions = ResolveHistoryColumns());
        private List<string> ResolveHistoryColumns()
        {
            var columnDescriptions = new List<string>();
            foreach (var prop in Entity.GetSqlProperties.Where(x => x.Property.DataType.Datatype != Enums.Datatype.ChildCollection))
            {
                if (MainInformation.Description.Required || !string.IsNullOrEmpty(prop.Property.Description) || prop.Property.DataType.Datatype == Enums.Datatype.Enum)
                {
                    var builder = new DescriptionHistoryColumnBuilder(prop, Entity, MainInformation);
                    columnDescriptions.Add(builder.Build());
                }
            }
            AddChangeTrackingColumns(columnDescriptions, $"{Entity.Name}_historie");
            columnDescriptions.Add(GetChangeTrackingDescription(SqlConstants.DeletedAtDescription, "DeletedAt", $"{Entity.Name}_historie"));
            columnDescriptions.Add(GetChangeTrackingDescription(SqlConstants.DeletedByDescription, "DeletedBy", $"{Entity.Name}_historie"));
            return columnDescriptions;
        }

        private List<string> ResolveColumns()
        {
            var columnDescriptions = new List<string>();
            foreach (var prop in Entity.GetSqlProperties.Where(x => x.Property.DataType.Datatype != Enums.Datatype.ChildCollection))
            {
                if (MainInformation.Description.Required || !string.IsNullOrEmpty(prop.Property.Description) || prop.Property.DataType.Datatype == Enums.Datatype.Enum)
                {
                    var builder = new DescriptionColumnBuilder(prop, Entity, MainInformation);
                    columnDescriptions.Add(builder.Build());
                }
            }
            AddChangeTrackingColumns(columnDescriptions, Entity.Name);
            return columnDescriptions;
        }

        protected virtual void AddChangeTrackingColumns(List<string> columnDescriptions, string tableName)
        {
            columnDescriptions.Add(GetChangeTrackingDescription(SqlConstants.IdDescription, "Id", tableName));
            columnDescriptions.Add(GetChangeTrackingDescription(SqlConstants.RowVersionDescription, "Rowversion", tableName));
            if (Entity.GenerateReadonlyGuid)
            {
                columnDescriptions.Add(GetChangeTrackingDescription(SqlConstants.UIdDescription, "UId", tableName));
            }
            columnDescriptions.Add(GetChangeTrackingDescription(SqlConstants.ModifiedOnDescription, "ModifiedOn", tableName));
            columnDescriptions.Add(GetChangeTrackingDescription(SqlConstants.ModifiedByDescription, "ModifiedBy", tableName));
            columnDescriptions.Add(GetChangeTrackingDescription(SqlConstants.ModifiedOnUTCDescription, "ModifiedOnUTC", tableName));
        }

        private string GetChangeTrackingDescription(string description, string columnName, string tableName)
        {
            var template = new Template();
            template.SetTemplate(SqlResources.DescriptionColumn);
            template.Fill(GetChangeTrackingParametersParameters(description, columnName, tableName));
            return template.GetText;
        }


        private TemplateParameter GetChangeTrackingParametersParameters(string description, string columnName, string tableName)
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, tableName);
            parameters.Add(SqlConstants.Description, description);
            parameters.Add(SqlConstants.Columnname, columnName);
            return parameters;
        }
    }
}
