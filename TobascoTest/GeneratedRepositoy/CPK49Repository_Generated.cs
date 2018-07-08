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
	public  partial class CPK49Repository : ICPK49Repository
	{
		private IGenericRepository<CPK49> _genericRepository;
public CPK49Repository(IGenericRepository<CPK49> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK49 Save(CPK49  cpk49)
{
	cpk49 = _genericRepository.Save(cpk49);	
	return cpk49;
}
public CPK49 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}