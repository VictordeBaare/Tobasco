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
	public  partial class CPK47Repository : ICPK47Repository
	{
		private IGenericRepository<CPK47> _genericRepository;
public CPK47Repository(IGenericRepository<CPK47> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK47 Save(CPK47  cpk47)
{
	cpk47 = _genericRepository.Save(cpk47);	
	return cpk47;
}
public CPK47 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}