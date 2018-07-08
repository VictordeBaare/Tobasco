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
	public  partial class CPK22Repository : ICPK22Repository
	{
		private IGenericRepository<CPK22> _genericRepository;
public CPK22Repository(IGenericRepository<CPK22> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK22 Save(CPK22  cpk22)
{
	cpk22 = _genericRepository.Save(cpk22);	
	return cpk22;
}
public CPK22 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}