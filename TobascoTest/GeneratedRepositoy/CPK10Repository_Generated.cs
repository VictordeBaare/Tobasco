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
	public  partial class CPK10Repository : ICPK10Repository
	{
		private IGenericRepository<CPK10> _genericRepository;
public CPK10Repository(IGenericRepository<CPK10> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK10 Save(CPK10  cpk10)
{
	cpk10 = _genericRepository.Save(cpk10);	
	return cpk10;
}
public CPK10 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}