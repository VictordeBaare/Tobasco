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
	public  partial class CPK15Repository : ICPK15Repository
	{
		private IGenericRepository<CPK15> _genericRepository;
public CPK15Repository(IGenericRepository<CPK15> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK15 Save(CPK15  cpk15)
{
	cpk15 = _genericRepository.Save(cpk15);	
	return cpk15;
}
public CPK15 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}