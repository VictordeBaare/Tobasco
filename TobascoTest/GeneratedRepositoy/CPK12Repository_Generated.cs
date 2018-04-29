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
	public  partial class CPK12Repository : ICPK12Repository
	{
		private IGenericRepository<CPK12> _genericRepository;
public CPK12Repository(IGenericRepository<CPK12> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK12 Save(CPK12  cpk12)
{
	cpk12 = _genericRepository.Save(cpk12);	
	return cpk12;
}
public CPK12 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}