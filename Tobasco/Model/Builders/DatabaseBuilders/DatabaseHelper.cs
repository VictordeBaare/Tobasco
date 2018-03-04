using System.Collections.Generic;
using System.Linq;
using Tobasco.Enums;
using Tobasco.Factories;
using Tobasco.Model.DatabaseProperties;

namespace Tobasco.Model.Builders.DatabaseBuilders
{
    public class DatabaseHelper
    {
        protected readonly Entity Entity;
        protected readonly Database Database;

        protected DatabaseHelper(Entity entity, Database database)
        {
            Entity = entity;
            Database = database;
        }

        public virtual string Name => Entity.Name;

        public virtual List<string> GetSqlUpdateParameters()
        {
            var list = new List<string>();

            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                list.Add($"{Name}.{selectSqlProperty.SelectSqlParameterNaam} = @{selectSqlProperty.SelectSqlParameterNaam}");
            }
            list.Add($"{Name}.ModifiedBy = ISNULL(@ModifiedBy, SUSER_SNAME())");
            list.Add($"{Name}.ModifiedOn = SYSDATETIME()");

            return list;
        }

        public virtual List<string> GetTableProperties()
        {
            return GetNonChildCollectionProperties.Select(prop => $",{prop.SelectSqlTableProperty}").ToList();
        }

        public IEnumerable<DatabaseProperty> GetChildProperties
        {
            get { return Entity.GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Child || x.Property.DataType.Datatype == Datatype.ReadonlyChild); }
        }

        public IEnumerable<DatabaseProperty> GetNonChildCollectionProperties
        {
            get { return Entity.GetSqlProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection); }
        }

        public virtual List<string> GetSqlParameterNames()
        {
            var list = new List<string>();

            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                list.Add($"{sqlprop.SelectSqlParameterNaam}");
            }
            list.Add("ModifiedBy");
            list.Add("ModifiedOn");

            return list;
        }

        public virtual List<string> GetSqlValueParameterNames()
        {
            var list = new List<string>();

            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                list.Add($"@{sqlprop.SelectSqlParameterNaam}");
            }
            list.Add("@ModifiedBy");
            list.Add("SYSDATETIME()");

            return list;
        }

        public virtual List<string> GetSqlParameters()
        {
            var list = new List<string>();

            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                list.Add($"@{sqlprop.SelectSqlParameter}");
            }

            return list;
        }
    }
}
