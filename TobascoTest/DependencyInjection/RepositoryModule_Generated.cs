using System;
using Ninject;
using Ninject.Modules;
using TobascoTest.GeneratedRepositoy;
using TobascoTest.GeneratedEntity;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.DependencyInjection
{
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506: AvoidExcessiveClassCoupling", Justification = "Generated file")]
    public partial class RepositoryModule : NinjectModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506: AvoidExcessiveClassCoupling", Justification = "Generated file")]
        public override void Load()
        {
            Bind<IChildCollectionObjectRepository>().To<ChildCollectionObjectRepository>();
            Bind<IGenericRepository<ChildCollectionObject>>().To<GenericRepository<ChildCollectionObject>>();
            Bind<IChildCollectionObjectDacRepository>().To<ChildCollectionObjectDacRepository>();
            Bind<IGenericRepository<ChildCollectionObjectDac>>().To<GenericRepository<ChildCollectionObjectDac>> ();
            Bind<IChildObjectRepository>().To<ChildObjectRepository>();
            Bind<IGenericRepository<ChildObject>>().To<GenericRepository<ChildObject>>();
            Bind<IChildObjectDacRepository>().To<ChildObjectDacRepository>();
            Bind<IGenericRepository<ChildObjectDac>>().To<GenericRepository<ChildObjectDac>> ();
            Bind<IFileMetOverervingRepository>().To<FileMetOverervingRepository>();
            Bind<IGenericRepository<FileMetOvererving>>().To<GenericRepository<FileMetOvererving>>();
            Bind<IFileMetOverervingDacRepository>().To<FileMetOverervingDacRepository>();
            Bind<IGenericRepository<FileMetOverervingDac>>().To<GenericRepository<FileMetOverervingDac>> ();
        }


    }
}
