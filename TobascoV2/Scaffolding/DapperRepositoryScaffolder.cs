using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Context;
using TobascoV2.Extensions;

namespace TobascoV2.Scaffolding
{
    internal class DapperRepositoryScaffolder : ScaffolderBase
    {
        internal override void AddUsingNamespaces(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            base.AddUsingNamespaces(entityContext, tobascoContext);
        }

        internal override void AddClass(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            ClassBuilder.StartClass($"{entityContext.Name}Repository", $"I{entityContext.Name}Repository");
        }

        internal override void AddConstructor(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            ClassBuilder.StartConstructor(entityContext.Name);
            ClassBuilder.EndConstructor();
        }

        internal override void AddMethods(EntityContext entityContext, ITobascoContext tobascoContext)
        {
            var entityParameter = entityContext.Name.GetParameterName();
            ClassBuilder.StartMethod("public", entityContext.Name, "Save", entityContext.Name.GetObjectWithParamaterName());
            ClassBuilder.AddMethodBody($"{entityParameter} = _genericRepository.Save({entityParameter});");
            ClassBuilder.AddMethodBody($"return {entityParameter};");
            ClassBuilder.EndMethod();
            ClassBuilder.AppendLine(string.Empty);
            ClassBuilder.StartMethod("public", entityContext.Name, "GetById", "long id");
            ClassBuilder.AddMethodBody("return _genericRepository.GetById(id);");
            ClassBuilder.EndMethod();
        }
    }
}
