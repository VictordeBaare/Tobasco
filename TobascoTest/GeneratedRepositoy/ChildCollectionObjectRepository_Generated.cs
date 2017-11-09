using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using static Dapper.SqlMapper;

namespace TobascoTest.GeneratedRepositoy
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class ChildCollectionObjectRepository : IChildCollectionObjectRepository
	{
		private IGenericRepository<ChildCollectionObject> _genericRepository;
public ChildCollectionObjectRepository(IGenericRepository<ChildCollectionObject> genericRepository)
{
	_genericRepository = genericRepository;
}		
				
		public ChildCollectionObject Save(ChildCollectionObject  childcollectionobject)
{
	childcollectionobject = _genericRepository.Save(childcollectionobject);	
		
	return childcollectionobject;
}
public ChildCollectionObject GetById(long id)
        {
            return _genericRepository.GetById(id);
        }

public ChildCollectionObject GetFullObjectById(long id)
{
    var parameters = new DynamicParameters();
    parameters.Add("id", id);
    return _genericRepository.QueryMultiple("[dbo].[ChildCollectionObject_GetFullById]", parameters, x => Read(x).Values).SingleOrDefault();
}
internal static Dictionary<long, ChildCollectionObject> Read(GridReader reader)
{
    return reader.Read<ChildCollectionObject>().ToDictionary(x => x.Id);      
}	
	}
}