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
    NinjectBinder<ICPK10Repository,CPK10Repository>();
NinjectBinder<IGenericRepository<CPK10>,GenericRepository<CPK10>>();
NinjectBinder<ICPK11Repository,CPK11Repository>();
NinjectBinder<IGenericRepository<CPK11>,GenericRepository<CPK11>>();
NinjectBinder<ICPK12Repository,CPK12Repository>();
NinjectBinder<IGenericRepository<CPK12>,GenericRepository<CPK12>>();
NinjectBinder<ICPK13Repository,CPK13Repository>();
NinjectBinder<IGenericRepository<CPK13>,GenericRepository<CPK13>>();
NinjectBinder<ICPK14Repository,CPK14Repository>();
NinjectBinder<IGenericRepository<CPK14>,GenericRepository<CPK14>>();
NinjectBinder<ICPK15Repository,CPK15Repository>();
NinjectBinder<IGenericRepository<CPK15>,GenericRepository<CPK15>>();
NinjectBinder<ICPK16Repository,CPK16Repository>();
NinjectBinder<IGenericRepository<CPK16>,GenericRepository<CPK16>>();
NinjectBinder<ICPK17Repository,CPK17Repository>();
NinjectBinder<IGenericRepository<CPK17>,GenericRepository<CPK17>>();
NinjectBinder<ICPK18Repository,CPK18Repository>();
NinjectBinder<IGenericRepository<CPK18>,GenericRepository<CPK18>>();
NinjectBinder<ICPK19Repository,CPK19Repository>();
NinjectBinder<IGenericRepository<CPK19>,GenericRepository<CPK19>>();
NinjectBinder<ICPK2Repository,CPK2Repository>();
NinjectBinder<IGenericRepository<CPK2>,GenericRepository<CPK2>>();
NinjectBinder<ICPK20Repository,CPK20Repository>();
NinjectBinder<IGenericRepository<CPK20>,GenericRepository<CPK20>>();
NinjectBinder<ICPK21Repository,CPK21Repository>();
NinjectBinder<IGenericRepository<CPK21>,GenericRepository<CPK21>>();
NinjectBinder<ICPK222Repository,CPK222Repository>();
NinjectBinder<IGenericRepository<CPK222>,GenericRepository<CPK222>>();
NinjectBinder<ICPK22Repository,CPK22Repository>();
NinjectBinder<IGenericRepository<CPK22>,GenericRepository<CPK22>>();
NinjectBinder<ICPK23Repository,CPK23Repository>();
NinjectBinder<IGenericRepository<CPK23>,GenericRepository<CPK23>>();
NinjectBinder<ICPK24Repository,CPK24Repository>();
NinjectBinder<IGenericRepository<CPK24>,GenericRepository<CPK24>>();
NinjectBinder<ICPK25Repository,CPK25Repository>();
NinjectBinder<IGenericRepository<CPK25>,GenericRepository<CPK25>>();
NinjectBinder<ICPK26Repository,CPK26Repository>();
NinjectBinder<IGenericRepository<CPK26>,GenericRepository<CPK26>>();
NinjectBinder<ICPK27Repository,CPK27Repository>();
NinjectBinder<IGenericRepository<CPK27>,GenericRepository<CPK27>>();
NinjectBinder<ICPK28Repository,CPK28Repository>();
NinjectBinder<IGenericRepository<CPK28>,GenericRepository<CPK28>>();
NinjectBinder<ICPK29Repository,CPK29Repository>();
NinjectBinder<IGenericRepository<CPK29>,GenericRepository<CPK29>>();
NinjectBinder<ICPK3Repository,CPK3Repository>();
NinjectBinder<IGenericRepository<CPK3>,GenericRepository<CPK3>>();
NinjectBinder<ICPK30Repository,CPK30Repository>();
NinjectBinder<IGenericRepository<CPK30>,GenericRepository<CPK30>>();
NinjectBinder<ICPK31Repository,CPK31Repository>();
NinjectBinder<IGenericRepository<CPK31>,GenericRepository<CPK31>>();
NinjectBinder<ICPK32Repository,CPK32Repository>();
NinjectBinder<IGenericRepository<CPK32>,GenericRepository<CPK32>>();
NinjectBinder<ICPK33Repository,CPK33Repository>();
NinjectBinder<IGenericRepository<CPK33>,GenericRepository<CPK33>>();
NinjectBinder<ICPK34Repository,CPK34Repository>();
NinjectBinder<IGenericRepository<CPK34>,GenericRepository<CPK34>>();
NinjectBinder<ICPK35Repository,CPK35Repository>();
NinjectBinder<IGenericRepository<CPK35>,GenericRepository<CPK35>>();
NinjectBinder<ICPK36Repository,CPK36Repository>();
NinjectBinder<IGenericRepository<CPK36>,GenericRepository<CPK36>>();
NinjectBinder<ICPK37Repository,CPK37Repository>();
NinjectBinder<IGenericRepository<CPK37>,GenericRepository<CPK37>>();
NinjectBinder<ICPK38Repository,CPK38Repository>();
NinjectBinder<IGenericRepository<CPK38>,GenericRepository<CPK38>>();
NinjectBinder<ICPK39Repository,CPK39Repository>();
NinjectBinder<IGenericRepository<CPK39>,GenericRepository<CPK39>>();
NinjectBinder<ICPK4Repository,CPK4Repository>();
NinjectBinder<IGenericRepository<CPK4>,GenericRepository<CPK4>>();
NinjectBinder<ICPK40Repository,CPK40Repository>();
NinjectBinder<IGenericRepository<CPK40>,GenericRepository<CPK40>>();
NinjectBinder<ICPK41Repository,CPK41Repository>();
NinjectBinder<IGenericRepository<CPK41>,GenericRepository<CPK41>>();
NinjectBinder<ICPK42Repository,CPK42Repository>();
NinjectBinder<IGenericRepository<CPK42>,GenericRepository<CPK42>>();
NinjectBinder<ICPK43Repository,CPK43Repository>();
NinjectBinder<IGenericRepository<CPK43>,GenericRepository<CPK43>>();
NinjectBinder<ICPK44Repository,CPK44Repository>();
NinjectBinder<IGenericRepository<CPK44>,GenericRepository<CPK44>>();
NinjectBinder<ICPK45Repository,CPK45Repository>();
NinjectBinder<IGenericRepository<CPK45>,GenericRepository<CPK45>>();
NinjectBinder<ICPK46Repository,CPK46Repository>();
NinjectBinder<IGenericRepository<CPK46>,GenericRepository<CPK46>>();
NinjectBinder<ICPK47Repository,CPK47Repository>();
NinjectBinder<IGenericRepository<CPK47>,GenericRepository<CPK47>>();
NinjectBinder<ICPK48Repository,CPK48Repository>();
NinjectBinder<IGenericRepository<CPK48>,GenericRepository<CPK48>>();
NinjectBinder<ICPK49Repository,CPK49Repository>();
NinjectBinder<IGenericRepository<CPK49>,GenericRepository<CPK49>>();
NinjectBinder<ICPK5Repository,CPK5Repository>();
NinjectBinder<IGenericRepository<CPK5>,GenericRepository<CPK5>>();
NinjectBinder<ICPK50Repository,CPK50Repository>();
NinjectBinder<IGenericRepository<CPK50>,GenericRepository<CPK50>>();
NinjectBinder<ICPK51Repository,CPK51Repository>();
NinjectBinder<IGenericRepository<CPK51>,GenericRepository<CPK51>>();
NinjectBinder<ICPK52Repository,CPK52Repository>();
NinjectBinder<IGenericRepository<CPK52>,GenericRepository<CPK52>>();
NinjectBinder<ICPK6Repository,CPK6Repository>();
NinjectBinder<IGenericRepository<CPK6>,GenericRepository<CPK6>>();
NinjectBinder<ICPK7Repository,CPK7Repository>();
NinjectBinder<IGenericRepository<CPK7>,GenericRepository<CPK7>>();
NinjectBinder<ICPK8Repository,CPK8Repository>();
NinjectBinder<IGenericRepository<CPK8>,GenericRepository<CPK8>>();
NinjectBinder<ICPK9Repository,CPK9Repository>();
NinjectBinder<IGenericRepository<CPK9>,GenericRepository<CPK9>>();
NinjectBinder<ICPK1Repository,CPK1Repository>();
NinjectBinder<IGenericRepository<CPK1>,GenericRepository<CPK1>>();
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