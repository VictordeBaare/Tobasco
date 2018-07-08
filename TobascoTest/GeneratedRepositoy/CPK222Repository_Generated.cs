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
	public  partial class CPK222Repository : ICPK222Repository
	{
		private IGenericRepository<CPK222> _genericRepository;
public CPK222Repository(IGenericRepository<CPK222> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK222 Save(CPK222  cpk222)
{
	cpk222 = _genericRepository.Save(cpk222);	
	return cpk222;
}
public CPK222 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}