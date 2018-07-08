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
	public  partial class CPK6Repository : ICPK6Repository
	{
		private IGenericRepository<CPK6> _genericRepository;
public CPK6Repository(IGenericRepository<CPK6> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK6 Save(CPK6  cpk6)
{
	cpk6 = _genericRepository.Save(cpk6);	
	return cpk6;
}
public CPK6 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}