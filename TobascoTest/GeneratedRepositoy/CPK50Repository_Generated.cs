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
	public  partial class CPK50Repository : ICPK50Repository
	{
		private IGenericRepository<CPK50> _genericRepository;
public CPK50Repository(IGenericRepository<CPK50> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK50 Save(CPK50  cpk50)
{
	cpk50 = _genericRepository.Save(cpk50);	
	return cpk50;
}
public CPK50 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}