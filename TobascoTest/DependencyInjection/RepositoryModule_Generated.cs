using System;
using System.CodeDom.Compiler;
using Ninject;
using Ninject.Modules;
using TobascoTest.GeneratedRepositoy;
using TobascoTest.GeneratedEntity;
using TobascoTest.IGenerateRepository;
using Ninject.Web.Common;

namespace TobascoTest.DependencyInjection
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506: AvoidExcessiveClassCoupling", Justification = "Generated file")]
	public  partial class RepositoryModule : NinjectModule
	{
				
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506: AvoidExcessiveClassCoupling", Justification = "Generated file")]
public override void Load()
{
    NinjectBinder<ICPKRepository,CPKRepository>();
NinjectBinder<IGenericRepository<CPK>,GenericRepository<CPK>>();
NinjectBinder<IChildCollectionObjectRepository,ChildCollectionObjectRepository>();
NinjectBinder<IGenericRepository<ChildCollectionObject>,GenericRepository<ChildCollectionObject>>();
NinjectBinder<IChildObjectRepository,ChildObjectRepository>();
NinjectBinder<IGenericRepository<ChildObject>,GenericRepository<ChildObject>>();
NinjectBinder<IFileMetOverervingRepository,FileMetOverervingRepository>();
NinjectBinder<IGenericRepository<FileMetOvererving>,GenericRepository<FileMetOvererving>>();
}
protected virtual void NinjectBinder<T, TY>() 
			where TY : class, T
			where T : class
{
    Bind<T>().To<TY>().InRequestScope();
}	
	}
}