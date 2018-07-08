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
	public  partial class CPK3Repository : ICPK3Repository
	{
		private IGenericRepository<CPK3> _genericRepository;
public CPK3Repository(IGenericRepository<CPK3> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK3 Save(CPK3  cpk3)
{
	cpk3 = _genericRepository.Save(cpk3);	
	return cpk3;
}
public CPK3 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}