using System;
using System.Collections.Generic;
using System.Text;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class TriggerBuilder : DatabaseHelper
    {
        public TriggerBuilder(Entity entity, Database database, EntityInformation information) : base(entity, database, information)
        {
        }

        public string Build()
        {
            var builder = new StringBuilder();
            builder.AppendLine(BuildUpdateTrigger());
            builder.AppendLine("GO");
            builder.AppendLine(BuildDeleteTrigger());
            return builder.ToString();
        }

        private string BuildDeleteTrigger()
        {
            var template = new Template();
            template.SetTemplate(GetResourceTriggerDelete);
            template.Fill(GetParameters(true));
            return template.GetText;
        }

        public virtual string GetResourceTriggerDelete => Entity.GenerateReadonlyGuid ? SqlResources.SqlTriggerDeleteWithUid : SqlResources.SqlTriggerDelete;
        public virtual string GetResourceTriggerUpdate => Entity.GenerateReadonlyGuid ? SqlResources.SqlTriggerUpdateWithUid : SqlResources.SqlTriggerUpdate;

        private string BuildUpdateTrigger()
        {
            var template = new Template();
            template.SetTemplate(GetResourceTriggerUpdate);
            template.Fill(GetParameters(false));
            return template.GetText;
        }

        private TemplateParameter GetParameters(bool isDelete)
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.StpPropertyNames, string.Join("," + Environment.NewLine, GetSqlParameterNames()));
            parameters.Add(SqlConstants.StpDeletetedPropertyNames, string.Join("," + Environment.NewLine, GetDeletedParameterNames(isDelete)));
            return parameters;
        }

        public override List<string> GetSqlParameterNames()
        {
            var parameters = base.GetSqlParameterNames();
            parameters.Insert(0, "Id");
            parameters.Insert(1, "[RowVersion]");
            if (Entity.GenerateReadonlyGuid)
            {
                parameters.Insert(1, "[UId]");
            }            
            parameters.Add("DeletedBy");
            parameters.Add("DeletedAt");
            return parameters;
        }

        protected virtual List<string> GetDeletedParameterNames(bool isDeleteTrigger)
        {
            var list = new List<string>();
            list.Add("Deleted.Id");
            if (Entity.GenerateReadonlyGuid)
            {
                list.Add("Deleted.[UId]");
            }
            list.Add("Deleted.[rowversion]");
            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                list.Add($"Deleted.{sqlprop.SelectSqlParameterNaam}");
            }
            list.Add("Deleted.ModifiedBy");
            list.Add("Deleted.ModifiedOn");
            if (isDeleteTrigger)
           { 
                list.Add("ISNULL(LTRIM(RTRIM(CONVERT(nvarchar(128), CONTEXT_INFO()))), SUSER_SNAME())");
                list.Add("SYSDATETIME()");
            }
            else
            {
                list.Add("NULL");
                list.Add("NULL");
            }

            return list;
        }
    }
}
