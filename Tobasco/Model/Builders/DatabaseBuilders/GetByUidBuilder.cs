using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Properties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class GetByUidBuilder : GetByIdHelper
    {
        public GetByUidBuilder(Entity entity, Database database) : base(entity, database)
        {
        }

        protected override string Template => SqlResources.GetFullObjectByEntityUid;

        public string Build()
        {
            var template = new Template();
            template.SetTemplate(Template);
            template.Fill(GetParameters());
            return template.CleanText;
        }

    }
}
