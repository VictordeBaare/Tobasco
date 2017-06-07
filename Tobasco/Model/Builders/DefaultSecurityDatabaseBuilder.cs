using System.Collections.Generic;
using System.Linq;
using Tobasco.Model.DatabaseProperties;

namespace Tobasco.Model.Builders
{
    public class DefaultSecurityDatabaseBuilder : DefaultDatabaseBuilder
    {
        private readonly Dac _dac;
        private IEnumerable<DatabaseProperty> _getSqlProperties;

        public DefaultSecurityDatabaseBuilder(Entity entity, Dac dac) : base(entity, dac.Database)
        {
            _dac = dac;
        }

        public override string Name => Entity.Name + "Dac";

        protected override IEnumerable<DatabaseProperty> GetSqlProperties
        {
            get
            {
                return _getSqlProperties ?? (_getSqlProperties = _dac.Properties.Select(x => Factory.GetDatabaseProperty(x)));
            }
        }
    }
}