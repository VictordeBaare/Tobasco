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
	public  partial class CPK9Repository : ICPK9Repository
	{
		private IGenericRepository<CPK9> _genericRepository;
public CPK9Repository(IGenericRepository<CPK9> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK9 Save(CPK9  cpk9)
{
	cpk9 = _genericRepository.Save(cpk9);	
	return cpk9;
}
public CPK9 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}