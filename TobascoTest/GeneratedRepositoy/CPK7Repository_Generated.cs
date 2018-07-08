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
	public  partial class CPK7Repository : ICPK7Repository
	{
		private IGenericRepository<CPK7> _genericRepository;
public CPK7Repository(IGenericRepository<CPK7> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK7 Save(CPK7  cpk7)
{
	cpk7 = _genericRepository.Save(cpk7);	
	return cpk7;
}
public CPK7 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}