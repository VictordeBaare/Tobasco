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
	public  partial class CPK43Repository : ICPK43Repository
	{
		private IGenericRepository<CPK43> _genericRepository;
public CPK43Repository(IGenericRepository<CPK43> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK43 Save(CPK43  cpk43)
{
	cpk43 = _genericRepository.Save(cpk43);	
	return cpk43;
}
public CPK43 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}