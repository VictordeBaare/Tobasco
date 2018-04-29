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
	public  partial class CPK2Repository : ICPK2Repository
	{
		private IGenericRepository<CPK2> _genericRepository;
public CPK2Repository(IGenericRepository<CPK2> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK2 Save(CPK2  cpk2)
{
	cpk2 = _genericRepository.Save(cpk2);	
	return cpk2;
}
public CPK2 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}