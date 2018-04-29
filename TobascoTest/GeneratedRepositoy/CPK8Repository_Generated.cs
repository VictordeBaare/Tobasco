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
	public  partial class CPK8Repository : ICPK8Repository
	{
		private IGenericRepository<CPK8> _genericRepository;
public CPK8Repository(IGenericRepository<CPK8> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK8 Save(CPK8  cpk8)
{
	cpk8 = _genericRepository.Save(cpk8);	
	return cpk8;
}
public CPK8 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}