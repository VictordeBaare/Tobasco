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
	public  partial class CPK27Repository : ICPK27Repository
	{
		private IGenericRepository<CPK27> _genericRepository;
public CPK27Repository(IGenericRepository<CPK27> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK27 Save(CPK27  cpk27)
{
	cpk27 = _genericRepository.Save(cpk27);	
	return cpk27;
}
public CPK27 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}