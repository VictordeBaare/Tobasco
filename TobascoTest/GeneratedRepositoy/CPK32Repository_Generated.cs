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
	public  partial class CPK32Repository : ICPK32Repository
	{
		private IGenericRepository<CPK32> _genericRepository;
public CPK32Repository(IGenericRepository<CPK32> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK32 Save(CPK32  cpk32)
{
	cpk32 = _genericRepository.Save(cpk32);	
	return cpk32;
}
public CPK32 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}