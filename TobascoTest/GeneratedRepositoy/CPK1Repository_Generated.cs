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
	public  partial class CPK1Repository : ICPK1Repository
	{
		private IGenericRepository<CPK1> _genericRepository;
public CPK1Repository(IGenericRepository<CPK1> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK1 Save(CPK1  cpk1)
{
	cpk1 = _genericRepository.Save(cpk1);	
	return cpk1;
}
public CPK1 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}