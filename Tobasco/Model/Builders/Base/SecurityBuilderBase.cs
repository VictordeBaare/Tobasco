using System.Collections.Generic;

namespace Tobasco.Model.Builders.Base
{
    public abstract class SecurityBuilderBase
    {
        protected readonly EntityHandler EntityHandler;
        protected readonly Entity Entity;
        protected readonly EntityInformation EntityInformation;

        protected SecurityBuilderBase(EntityHandler entityHandler, EntityInformation entityInformation)
        {
            Entity = entityHandler.Entity;
            EntityHandler = entityHandler;
            EntityInformation = entityInformation;
        }

        public abstract IEnumerable<FileBuilder.OutputFile> Build();

        protected Model.Security GetSecurityElement()
        {
            if (Entity.Security != null)
            {
                return Entity.Security;
            }
            return EntityInformation.Security;
        }

    }
}