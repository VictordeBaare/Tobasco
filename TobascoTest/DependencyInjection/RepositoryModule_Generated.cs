using System;
using Ninject;
using Ninject.Modules;
using TobascoTest.GeneratedRepositoy;
using TobascoTest.GeneratedEntity;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.DependencyInjection
{
    [Serializable]
    public partial class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IChildCollectionObjectRepository>().To<ChildCollectionObjectRepository>();
            Bind<IGenericRepository<ChildCollectionObject>>().To<GenericRepository<ChildCollectionObject>>();
            Bind<IChildObjectRepository>().To<ChildObjectRepository>();
            Bind<IGenericRepository<ChildObject>>().To<GenericRepository<ChildObject>>();
            Bind<IFileMetOverervingRepository>().To<FileMetOverervingRepository>();
            Bind<IGenericRepository<FileMetOvererving>>().To<GenericRepository<FileMetOvererving>>();
        }


    }
}
