using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Factories;
using Tobasco.Model.DatabaseProperties;
using Tobasco.Enums;

namespace Tobasco.Model.Builders.Base
{
    public abstract class DatabaseBuilderBase
    {
        protected readonly Entity Entity;
        protected readonly Database Database;
        protected readonly DatabasePropertyFactory Factory;
        private IEnumerable<DatabaseProperty> _getSqlProperties;

        protected DatabaseBuilderBase(Entity entity, Database database)
        {
            Entity = entity;
            Database = database;
            Factory = new DatabasePropertyFactory();
        }

        public virtual string Name { get; set; }

        protected virtual IEnumerable<DatabaseProperty> GetSqlProperties
        {
            get
            {
                return _getSqlProperties ?? (_getSqlProperties = Entity.Properties.Select(x => Factory.GetDatabaseProperty(x)));
            }
        }

        protected IEnumerable<DatabaseProperty> GetChildProperties
        {
            get { return GetSqlProperties.Where(x => x.Property.DataType.Datatype == Datatype.Child); }
        }

        protected IEnumerable<DatabaseProperty> GetNonChildCollectionProperties
        {
            get { return GetSqlProperties.Where(x => x.Property.DataType.Datatype != Datatype.ChildCollection); }
        }

        public abstract IEnumerable<FileBuilder.OutputFile> Build();
    }
}
