using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TobascoV2.Constants;
using TobascoV2.Context;
using TobascoV2.Extensions;

namespace TobascoV2.Scaffolding
{
    internal class DapperRepositoryScaffolder : ScaffolderClassBase
    {
        internal override void AddUsingNamespaces(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            
        }

        internal override void AddClass(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            Builder.StartClass($"{entityContext.Name}Repository", $"I{entityContext.Name}Repository");
        }

        internal override void AddFields(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            Builder.AddField("private readonly IGenericRepository _genericRepository");
        }

        internal override void AddConstructor(XmlEntity entityContext, ITobascoContext tobascoContext)
        {
            var childs = entityContext.GetChilds().Concat(entityContext.GetChildCollections());

            Builder.StartConstructor(entityContext.Name, GetConstructorParameters(childs).ToArray());
            Builder.AddConstructorBody("_genericRepository = genericRepository;");
            foreach (var child in childs)
            {
                Builder.AddConstructorBody($"{child.Name.GetFieldName()} = {child.Name.GetParameterName()};");
            }
            Builder.EndConstructor();

            IEnumerable<string> GetConstructorParameters(IEnumerable<XmlProperty> childProperties)
            {
                var parameters = new List<string> { "IGenericRepository genericRepository" };
                parameters.AddRange(childProperties.Select(x => $"I{x.Name}Repository {x.Name.GetParameterName()}"));
                return parameters;
            }
        }

        internal override void AddMethods(XmlEntity entityContext, ITobascoContext tobascoContext)
        {             
            var entityParameter = entityContext.Name.GetParameterName();
            Builder.StartMethod("public", entityContext.Name, "Save", entityContext.Name.GetObjectWithParamaterName());
            foreach(var child in entityContext.GetChilds())
            {
                Builder.AddMethodBody($"if (var {entityContext.Name}.{child.Name} != null)");
                Builder.AddMethodBody("{");
                Builder.AppendLine($"{entityContext.Name}.{child.Name} = {child.Name.GetFieldName()}.Save({child.Name});", Indent.ForEachBody);
                Builder.AddMethodBody("}");
            }
            Builder.AddMethodBody($"{entityParameter} = _genericRepository.Save({entityParameter});");
            foreach(var childCollection in entityContext.GetChildCollections())
            {
                Builder.AddMethodBody($"foreach (var itemToSave = {entityContext.Name}.{childCollection.Name})");
                Builder.AddMethodBody("{");
                Builder.AppendLine($"itemToSave.{entityContext.Name}Id = {entityContext.Name.GetObjectWithParamaterName()};", Indent.ForEachBody);
                Builder.AppendLine($"{childCollection.Name.GetParameterName()}.Save(itemToSave);", Indent.ForEachBody);
                Builder.AddMethodBody("}");
            }
            Builder.AddMethodBody($"return {entityParameter};");
            Builder.EndMethod();
            Builder.AppendLine(string.Empty);
            Builder.StartMethod("public", entityContext.Name, "GetById", "long id");
            Builder.AddMethodBody("return _genericRepository.GetById(id);");
            Builder.EndMethod();
        }
    }
}
