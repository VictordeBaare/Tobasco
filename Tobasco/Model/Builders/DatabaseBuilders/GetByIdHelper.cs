using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Model.DatabaseProperties;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public abstract class GetByIdHelper : DatabaseHelper
    {
        protected IEnumerable<string> _getDeclareChilds;
        protected IEnumerable<string> _getChildCollectionByParentId;
        protected IEnumerable<string> _getChild_ByIdStp;

        public override string Name => Entity.Name;

        protected abstract string Template { get; }

        public GetByIdHelper(Entity entity, Database database) : base(entity, database)
        {
        }

        public virtual TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.StpPropertyNames, GetNonChildCollectionProperties.OrderBy(x => x.Property.Name).ThenByDescending(x => x.Property.DataType.Datatype != Datatype.Child).ThenByDescending(x => x.Property.DataType.Datatype != Datatype.Child).Select(x =>$"{x.SelectSqlParameterNaam},"));
            parameters.Add(SqlConstants.DeclareChilds, DeclareChilds);
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.Childs_GetById, GetChild_ByIdStp);
            parameters.Add(SqlConstants.ChildCollection_GetByParentIdStp, GetChildCollectionGetByParentId);

            return parameters;
        }
        
        public virtual IEnumerable<string> GetChildCollectionGetByParentId
        {
            get
            {
                return _getChildCollectionByParentId ?? (_getChildCollectionByParentId = Entity.Properties
                    .Where(x => x.DataType.Datatype == Datatype.ChildCollection)
                    .Select(x => $"EXEC {x.DataType.Type}_GetFullBy{Entity.Name}Id @id"));
            }
        }
        
        public virtual IEnumerable<string> DeclareChilds
        {
            get
            {
                return _getDeclareChilds ?? (_getDeclareChilds = Entity.GetSqlProperties
                    .Where(x => x.Property.DataType.Datatype == Datatype.ReadonlyChild || x.Property.DataType.Datatype == Datatype.Child)
                    .Select(x => GetChildDeclareId(x, "@id")));
            }
        }
        
        public string GetChildDeclareId(DatabaseProperty property, string parameterName)
        {
            StringBuilder stringbuilder = new StringBuilder();
            stringbuilder.AppendLine($"DECLARE @{property.SelectSqlParameterNaam} as bigint;");
            stringbuilder.AppendLine($"SELECT @{property.SelectSqlParameterNaam} = {property.SelectSqlParameterNaam}");
            stringbuilder.AppendLine($"  FROM [dbo].{Entity.Name}");
            stringbuilder.AppendLine($" WHERE {Entity.Name}.Id = {parameterName};");
            return stringbuilder.ToString();
        }
        
        public virtual IEnumerable<string> GetChild_ByIdStp
        {
            get
            {
                return _getChild_ByIdStp ?? (_getChild_ByIdStp = Entity.GetSqlProperties
                    .Where(x => x.Property.DataType.Datatype == Datatype.ReadonlyChild || x.Property.DataType.Datatype == Datatype.Child)
                    .Select(x => $"EXEC [dbo].{x.Property.DataType.Type}_GetFullById @{x.SelectSqlParameterNaam};"));
            }
        }
    }
}
