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
        protected readonly EntityHandler Entity;
        protected IEnumerable<DatabaseProperty> _getSqlProperties;
        protected readonly DatabasePropertyFactory Factory;

        protected DatabaseBuilderBase(EntityHandler entity)
        {
            Entity = entity;
            Factory = new DatabasePropertyFactory();
        }

        protected IEnumerable<DatabaseProperty> GetSqlProperties
        {
            get
            {
                if (_getSqlProperties == null)
                {
                    _getSqlProperties = Entity.Entity.Properties.Select(x => Factory.GetDatabaseProperty(x));
                }
                return _getSqlProperties;
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
