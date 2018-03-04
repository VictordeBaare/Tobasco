using System.Collections.Generic;
using Tobasco.Manager;

namespace Tobasco.Model.Builders.Base
{
    public abstract class ConnectionfactoryBuilderBase
    {
        protected ConnectionfactoryBuilderBase()
        {
        }

        protected FileLocation classlocation => MainInfoManager.EntityInformation.ConnectionFactory.FileLocation;
        protected FileLocation interfacelocation => MainInfoManager.EntityInformation.ConnectionFactory.InterfaceLocation;

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
