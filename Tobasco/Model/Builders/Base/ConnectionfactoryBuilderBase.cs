using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Model.Builders.Base
{
    public abstract class ConnectionfactoryBuilderBase
    {
        protected readonly EntityInformation Information;

        protected ConnectionfactoryBuilderBase(EntityInformation information)
        {
            Information = information;
        }

        protected FileLocation classlocation => Information.ConnectionFactory.FileLocation;
        protected FileLocation interfacelocation => Information.ConnectionFactory.InterfaceLocation;

        public virtual string GetConnectionFactoryName
        {
            get
            {
                return "ConnectionFactory";
            }
        }

        public virtual string GetConnectionFactoryInterfaceName
        {
            get
            {
                return "IConnectionFactory";
            }
        }

        public abstract IEnumerable<FileBuilder.OutputFile> Build();
    }
}
