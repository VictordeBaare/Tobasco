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
	public  partial class CPK4Repository : ICPK4Repository
	{
		private IGenericRepository<CPK4> _genericRepository;
public CPK4Repository(IGenericRepository<CPK4> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK4 Save(CPK4  cpk4)
{
	cpk4 = _genericRepository.Save(cpk4);	
	return cpk4;
}
public CPK4 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}