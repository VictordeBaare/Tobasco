using System;
using System.CodeDom.Compiler;
using TobascoTest.IGenerateRepository;
using static Dapper.SqlMapper;
using Dapper;
using System.Linq;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public partial interface IChildCollectionObjectRepository
	{
			
		ChildCollectionObject Save(ChildCollectionObject childcollectionobject);
ChildCollectionObject GetById(long id);
ChildCollectionObject GetFullObjectById(long id);
ChildCollectionObject GetFullObjectByUId(Guid uid);	
	}
}