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
	public  partial class CPK53Repository : ICPK53Repository
	{
		private IGenericRepository<CPK53> _genericRepository;
public CPK53Repository(IGenericRepository<CPK53> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK53 Save(CPK53  cpk53)
{
	cpk53 = _genericRepository.Save(cpk53);	
	return cpk53;
}
public CPK53 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}