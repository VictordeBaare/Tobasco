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
	public  partial class CPK24Repository : ICPK24Repository
	{
		private IGenericRepository<CPK24> _genericRepository;
public CPK24Repository(IGenericRepository<CPK24> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK24 Save(CPK24  cpk24)
{
	cpk24 = _genericRepository.Save(cpk24);	
	return cpk24;
}
public CPK24 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}