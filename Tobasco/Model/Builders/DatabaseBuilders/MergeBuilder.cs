using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Constants;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class MergeBuilder : DatabaseHelper
    {
        public MergeBuilder(Entity entity, Database database) : base(entity, database)
        {

        }

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(SqlResources.SqlMergeStp);
            template.Fill(GetParameters());

            return template.GetText;
        }

        private TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Entity.Name);
            parameters.Add(SqlConstants.TableProperties, GetTableProperties());
            parameters.Add(SqlConstants.StpPropertyNames, GetSqlParameterNames(string.Empty));
            parameters.Add(SqlConstants.StpMergeSourcePropertyNames, GetSqlParameterNames("[Source]."));
            parameters.Add(SqlConstants.StpMergeOutputAction, GetMergeOuputActions());
            parameters.Add(SqlConstants.UpdateSetTableParemeters, GetSqlUpdateParameters());
            parameters.Add(SqlConstants.MergeOutputParameters, GetSqlParameterNames("#output."));
            return parameters;
        }

        private List<string> GetMergeOuputActions()
        {
            var sqlParameters = GetNonChildCollectionProperties;

            return sqlParameters.Select(x => $",IIF($action = 'DELETE', deleted.{x.SelectSqlParameterNaam}, inserted.{x.SelectSqlParameterNaam})").ToList();
        }

        protected override List<string> GetSqlUpdateParameters()
        {
            var list = new List<string>();

            foreach (var selectSqlProperty in GetNonChildCollectionProperties)
            {
                list.Add($"[Target].{selectSqlProperty.SelectSqlParameterNaam} = [Source].{selectSqlProperty.SelectSqlParameterNaam},");
            }

            return list;
        }
    }
}
