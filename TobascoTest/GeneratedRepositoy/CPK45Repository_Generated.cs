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
	public  partial class CPK45Repository : ICPK45Repository
	{
		private IGenericRepository<CPK45> _genericRepository;
public CPK45Repository(IGenericRepository<CPK45> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK45 Save(CPK45  cpk45)
{
	cpk45 = _genericRepository.Save(cpk45);	
	return cpk45;
}
public CPK45 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}