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
	public  partial class CPK18Repository : ICPK18Repository
	{
		private IGenericRepository<CPK18> _genericRepository;
public CPK18Repository(IGenericRepository<CPK18> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK18 Save(CPK18  cpk18)
{
	cpk18 = _genericRepository.Save(cpk18);	
	return cpk18;
}
public CPK18 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}