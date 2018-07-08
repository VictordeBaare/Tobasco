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
	public  partial class CPK26Repository : ICPK26Repository
	{
		private IGenericRepository<CPK26> _genericRepository;
public CPK26Repository(IGenericRepository<CPK26> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK26 Save(CPK26  cpk26)
{
	cpk26 = _genericRepository.Save(cpk26);	
	return cpk26;
}
public CPK26 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}