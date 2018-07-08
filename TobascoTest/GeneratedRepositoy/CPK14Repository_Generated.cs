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
	public  partial class CPK14Repository : ICPK14Repository
	{
		private IGenericRepository<CPK14> _genericRepository;
public CPK14Repository(IGenericRepository<CPK14> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK14 Save(CPK14  cpk14)
{
	cpk14 = _genericRepository.Save(cpk14);	
	return cpk14;
}
public CPK14 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}