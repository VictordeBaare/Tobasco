using System.Collections.Generic;
using System.Linq;
using Tobasco.Constants;
using Tobasco.Enums;
using Tobasco.Templates;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class GetByIdHelper : DatabaseHelper
    {
        private IEnumerable<string> _getDeclareChilds;
        private IEnumerable<string> _getChildCollectionByParentId;
        private IEnumerable<string> _getChild_ByIdStp;

        public override string Name => Entity.Name;

        public GetByIdHelper(Entity entity, Database database) : base(entity, database)
        {
        }

        protected virtual TemplateParameter GetParameters()
        {
            var parameters = new TemplateParameter();
            parameters.Add(SqlConstants.StpPropertyNames, GetNonChildCollectionProperties.OrderBy(x => x.Property.Name).ThenByDescending(x => x.Property.DataType.Datatype != Datatype.Child).ThenByDescending(x => x.Property.DataType.Datatype != Datatype.Child).Select(x =>$"{x.SelectSqlParameterNaam},"));
            parameters.Add(SqlConstants.DeclareChilds, DeclareChilds);
            parameters.Add(SqlConstants.TableName, Name);
            parameters.Add(SqlConstants.Childs_GetById, GetChild_ByIdStp);
            parameters.Add(SqlConstants.ChildCollection_GetByParentIdStp, GetChildCollectionGetByParentId);

            return parameters;
        }

        protected virtual IEnumerable<string> GetChildCollectionGetByParentId
        {
            get
            {
                return _getChildCollectionByParentId ?? (_getChildCollectionByParentId = Entity.Properties
                    .Where(x => x.DataType.Datatype == Datatype.ChildCollection)
                    .Select(x => $"EXEC {x.DataType.Type}_GetFullBy{Entity.Name} @id"));
            }
        }

        protected virtual IEnumerable<string> DeclareChilds
        {
            get
            {
                return _getDeclareChilds ?? (_getDeclareChilds = Entity.Properties
                    .Where(x => x.DataType.Datatype == Datatype.ReadonlyChild || x.DataType.Datatype == Datatype.Child)
                    .Select(x => $"DECLARE @{x.Name} as bigint;"));
            }
        }

        protected virtual IEnumerable<string> GetChild_ByIdStp
        {
            get
            {
                return _getChild_ByIdStp ?? (_getChild_ByIdStp = Entity.Properties
                    .Where(x => x.DataType.Datatype == Datatype.ReadonlyChild || x.DataType.Datatype == Datatype.Child)
                    .Select(x => $"EXEC [dbo].{x.DataType.Type}_GetFullById @{x.Name};"));
            }
        }
    }
}
