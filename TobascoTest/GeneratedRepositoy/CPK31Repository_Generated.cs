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
	public  partial class CPK31Repository : ICPK31Repository
	{
		private IGenericRepository<CPK31> _genericRepository;
public CPK31Repository(IGenericRepository<CPK31> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK31 Save(CPK31  cpk31)
{
	cpk31 = _genericRepository.Save(cpk31);	
	return cpk31;
}
public CPK31 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}