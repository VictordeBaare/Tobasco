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
	public  partial class CPK36Repository : ICPK36Repository
	{
		private IGenericRepository<CPK36> _genericRepository;
public CPK36Repository(IGenericRepository<CPK36> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK36 Save(CPK36  cpk36)
{
	cpk36 = _genericRepository.Save(cpk36);	
	return cpk36;
}
public CPK36 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}