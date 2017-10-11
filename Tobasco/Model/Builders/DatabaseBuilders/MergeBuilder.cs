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
            template.SetTemplate(Resources.SqlMergeStp);
            template.Fill(GetParameters());

            return template.GetText;
        }

        private TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.TableName, Entity.Name);

            return parameters;
        }
    }
}
