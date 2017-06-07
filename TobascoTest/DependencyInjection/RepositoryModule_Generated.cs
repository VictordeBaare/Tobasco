using System;
using Ninject;
using Ninject.Modules;
using TobascoTest.GeneratedRepositoy;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.DependencyInjection
{
    [Serializable]
    public partial class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IChildCollectionObjectRepository>().To<ChildCollectionObjectRepository>();
            Bind<IChildObjectRepository>().To<ChildObjectRepository>();
            Bind<IFileMetOverervingRepository>().To<FileMetOverervingRepository>();
        }


    }
}
