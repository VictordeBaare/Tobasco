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
	public  partial class CPK56Repository : ICPK56Repository
	{
		private IGenericRepository<CPK56> _genericRepository;
public CPK56Repository(IGenericRepository<CPK56> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK56 Save(CPK56  cpk56)
{
	cpk56 = _genericRepository.Save(cpk56);	
	return cpk56;
}
public CPK56 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}