using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class GetByIdBuilder : GetByIdHelper
    {
        public GetByIdBuilder(Entity entity, Database database) : base(entity, database)
        {
        }


        public string Build()
        {
            var template = new Template();
            template.SetTemplate(SqlResources.GetFullObjectByEntity);
            template.Fill(GetParameters());
            return template.CleanText;
        }
    }
}
