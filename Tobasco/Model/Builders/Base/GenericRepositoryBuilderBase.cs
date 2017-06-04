using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobasco.Model.Builders.Base
{
    public abstract class GenericRepositoryBuilderBase
    {
        protected readonly EntityInformation Information;

        protected GenericRepositoryBuilderBase(EntityInformation information)
        {
            Information = information;
        }

        public FileLocation Classlocation => Information.GenericRepository.FileLocation;
        public FileLocation Interfacelocation => Information.GenericRepository.InterfaceLocation;

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
