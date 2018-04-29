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
	public  partial class CPK20Repository : ICPK20Repository
	{
		private IGenericRepository<CPK20> _genericRepository;
public CPK20Repository(IGenericRepository<CPK20> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK20 Save(CPK20  cpk20)
{
	cpk20 = _genericRepository.Save(cpk20);	
	return cpk20;
}
public CPK20 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}