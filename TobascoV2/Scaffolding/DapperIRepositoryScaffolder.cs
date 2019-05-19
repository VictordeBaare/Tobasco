using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Context;

namespace TobascoV2.Scaffolding
{
    internal class DapperIRepositoryScaffolder : ScaffolderClassBase
    {
        internal override void AddClass(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            Builder.StartInterface($"I{entityContext.Name}Repository");
        }

        internal override void AddUsingNamespaces(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            base.AddUsingNamespaces(entityContext, tobascoContext);
        }
    }
}
