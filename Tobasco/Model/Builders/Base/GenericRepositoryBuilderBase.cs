using System.Collections.Generic;
using Tobasco.Manager;

namespace Tobasco.Model.Builders.Base
{
    public abstract class GenericRepositoryBuilderBase
    {
        protected GenericRepositoryBuilderBase()
        {
        }

        public FileLocation Classlocation => MainInfoManager.EntityInformation.GenericRepository.FileLocation;
        public FileLocation Interfacelocation => MainInfoManager.EntityInformation.GenericRepository.InterfaceLocation;

        public virtual string GetGenericRepositoryName
        {
            get { return "GenericRepository"; }
        }

        public virtual string GetInterfaceGenericRepositoryName
        {
            get { return "IGenericRepository"; }
        }

        public abstract IEnumerable<FileBuilder.OutputFile> Build();
    }
}
