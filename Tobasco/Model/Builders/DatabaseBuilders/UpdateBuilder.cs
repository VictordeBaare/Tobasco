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
    public class UpdateBuilder : DatabaseHelper
    {
        public UpdateBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(GetTemplate);
            template.Fill(UpdateTemplateParameters());

            return template.GetText;
        }

        public virtual string GetTemplate => SqlResources.SqlUpdateStp;

        public virtual TemplateParameter UpdateTemplateParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.StpParameter, string.Join("," + Environment.NewLine, GetSqlParameters()));
            parameters.Add(SqlConstants.StpParameterName, string.Join("," + Environment.NewLine, GetSqlValueParameterNames()));
            parameters.Add(SqlConstants.StpPropertyNames, string.Join("," + Environment.NewLine, GetSqlParameterNames()));
            parameters.Add(SqlConstants.UpdateSetTableParemeters, string.Join("," + Environment.NewLine, GetSqlUpdateParameters()));

            return parameters;
        }

        public override List<string> GetSqlParameters()
        {
            var parameters = base.GetSqlParameters();
            parameters.Insert(0, "@Id [bigint]");
            parameters.Add("@RowVersion [rowversion]");
            parameters.Add("@ModifiedBy nvarchar(256)");

            return parameters;
        }
    }
}
