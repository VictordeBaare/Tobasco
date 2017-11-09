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
	public  partial class ChildObjectRepository : IChildObjectRepository
	{
		private IGenericRepository<ChildObject> _genericRepository;
public ChildObjectRepository(IGenericRepository<ChildObject> genericRepository)
{
	_genericRepository = genericRepository;
}		
				
		public ChildObject Save(ChildObject  childobject)
{
	childobject = _genericRepository.Save(childobject);	
		
	return childobject;
}
public ChildObject GetById(long id)
        {
            return _genericRepository.GetById(id);
        }

public ChildObject GetFullObjectById(long id)
{
    var parameters = new DynamicParameters();
    parameters.Add("id", id);
    return _genericRepository.QueryMultiple("[dbo].[ChildObject_GetFullById]", parameters, x => Read(x).Values).SingleOrDefault();
}
internal static Dictionary<long, ChildObject> Read(GridReader reader)
{
    return reader.Read<ChildObject>().ToDictionary(x => x.Id);      
}	
	}
}