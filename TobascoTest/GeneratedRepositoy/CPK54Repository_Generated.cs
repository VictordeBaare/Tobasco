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
	public  partial class CPK54Repository : ICPK54Repository
	{
		private IGenericRepository<CPK54> _genericRepository;
public CPK54Repository(IGenericRepository<CPK54> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK54 Save(CPK54  cpk54)
{
	cpk54 = _genericRepository.Save(cpk54);	
	return cpk54;
}
public CPK54 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}