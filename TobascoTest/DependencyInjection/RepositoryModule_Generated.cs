using System;
using System.CodeDom.Compiler;
using Ninject;
using Ninject.Modules;
using TobascoTest.GeneratedRepositoy;
using TobascoTest.GeneratedEntity;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.DependencyInjection
{
    [GeneratedCode("Tobasco", "1.0.0.0")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506: AvoidExcessiveClassCoupling", Justification = "Generated file")]
    public partial class RepositoryModule : NinjectModule
    {




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506: AvoidExcessiveClassCoupling", Justification = "Generated file")]
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