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
	public  partial class CPKRepository : ICPKRepository
	{
		private IGenericRepository<CPK> _genericRepository;
public CPKRepository(IGenericRepository<CPK> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK Save(CPK  cpk)
{
	cpk = _genericRepository.Save(cpk);	
	return cpk;
}
public CPK GetById(long id)
        {
            return _genericRepository.GetById(id);
        }

public CPK GetFullObjectById(long id)
{
    var parameters = new DynamicParameters();
    parameters.Add("id", id);
    return _genericRepository.ExecuteQueryMultiple("[dbo].[CPK_GetFullById]", parameters, x => Read(x).Values).SingleOrDefault();
}
internal static Dictionary<long, CPK> Read(GridReader reader)
{
	var aaaDict = CPK1Repository.Read(reader);

		var items = reader.Read((CPK item,long? aaa) =>
    {
        if (aaa.HasValue && aaaDict.ContainsKey(aaa.Value))
{
item.aaa = aaaDict[aaa.Value];
}

        

        return item;
    }, splitOn: "aaaId");
		
    return items.ToDictionary(x => x.Id);        
}	
	}
}