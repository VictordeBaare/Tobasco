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
        protected readonly DatabasePropertyFactory Factory;
        private IEnumerable<DatabaseProperty> _getSqlProperties;

        protected DatabaseHelper(Entity entity, Database database)
        {
            Entity = entity;
            Database = database;
            Factory = new DatabasePropertyFactory();
        }

        public virtual string Name => Entity.Name;

        protected virtual IEnumerable<DatabaseProperty> GetSqlProperties
        {
            get
            {
                return _getSqlProperties ?? (_getSqlProperties = Entity.Properties.Select(x => Factory.GetDatabaseProperty(x)));
            }
        }

        protected virtual List<string> GetSqlUpdateParameters()
        {
            var list = new List<string>();

            foreach (DatabaseProperty selectSqlProperty in GetNonChildCollectionProperties)
            {
                list.Add($"{Name}.{selectSqlProperty.SelectSqlParameterNaam} = @{selectSqlProperty.SelectSqlParameterNaam},");
            }

            return list;
        }

        protected virtual List<string> GetTableProperties()
        {
            return GetNonChildCollectionProperties.Select(prop => $",{prop.SelectSqlTableProperty}").ToList();
        }

        protected IEnumerable<DatabaseProperty> GetChildProperties
        {
            get { return GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Child || x.Property.DataType.Datatype == Datatype.ReadonlyChild); }
        }

        protected IEnumerable<DatabaseProperty> GetNonChildCollectionProperties
        {
            get { return GetSqlProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection); }
        }

        protected virtual List<string> GetSqlParameterNames(string leading = null)
        {
            var list = new List<string>();

            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                list.Add($"{leading}{sqlprop.SelectSqlParameterNaam},");
            }

            return list;
        }

        protected virtual List<string> GetSqlParameters()
        {
            var list = new List<string>();

            foreach (var sqlprop in GetNonChildCollectionProperties)
            {
                list.Add($"@{sqlprop.SelectSqlParameter},");
            }

            return list;
        }
    }
}
