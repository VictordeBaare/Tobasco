using System;
using System.Collections.Generic;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class InsertBuilder : DatabaseHelper
    {
        public InsertBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(Entity.GenerateReadonlyGuid ? GetInsertWithUid : GetInsert);
            template.Fill(TemplateParameters());

            return template.GetText;
        }

        public virtual TemplateParameter TemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.StpParameter, string.Join("," + Environment.NewLine, GetSqlParameters()));
            parameters.Add(SqlConstants.StpParameterName, string.Join("," + Environment.NewLine, GetSqlValueParameterNames()));
            parameters.Add(SqlConstants.StpPropertyNames, string.Join("," + Environment.NewLine, GetSqlParameterNames()));
            return parameters;
        }

        public virtual string GetInsertWithUid => SqlResources.SqlInsertStpWithUid;
        public virtual string GetInsert => SqlResources.SqlInsertStp;

        public override List<string> GetSqlParameters()
        {
            var parameters=  base.GetSqlParameters();
            parameters.Insert(0, "@Id bigint output");
            parameters.Add("@ModifiedBy nvarchar(256)");
            return parameters;
        }
    }
}
