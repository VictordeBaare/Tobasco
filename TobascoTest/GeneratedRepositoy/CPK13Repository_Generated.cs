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
	public  partial class CPK13Repository : ICPK13Repository
	{
		private IGenericRepository<CPK13> _genericRepository;
public CPK13Repository(IGenericRepository<CPK13> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK13 Save(CPK13  cpk13)
{
	cpk13 = _genericRepository.Save(cpk13);	
	return cpk13;
}
public CPK13 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}