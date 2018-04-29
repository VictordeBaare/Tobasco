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
	public  partial class CPK44Repository : ICPK44Repository
	{
		private IGenericRepository<CPK44> _genericRepository;
public CPK44Repository(IGenericRepository<CPK44> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK44 Save(CPK44  cpk44)
{
	cpk44 = _genericRepository.Save(cpk44);	
	return cpk44;
}
public CPK44 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}