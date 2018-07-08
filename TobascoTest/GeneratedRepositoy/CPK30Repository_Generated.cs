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
	public  partial class CPK30Repository : ICPK30Repository
	{
		private IGenericRepository<CPK30> _genericRepository;
public CPK30Repository(IGenericRepository<CPK30> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK30 Save(CPK30  cpk30)
{
	cpk30 = _genericRepository.Save(cpk30);	
	return cpk30;
}
public CPK30 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}