using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobasco.Enums;

namespace Tobasco.Factories
{
    public static class OrmPropertyFactory
    {
        public static PropertyFactory GeefPropertyBuilder(OrmType ormType)
        {
            switch (ormType)
            {
                case OrmType.Dapper:
                    return new DapperPropertyFactory();
                default:
                    return new PropertyFactory();
            }
        }
    }
}
