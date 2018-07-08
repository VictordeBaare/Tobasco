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
	public  partial class CPK46Repository : ICPK46Repository
	{
		private IGenericRepository<CPK46> _genericRepository;
public CPK46Repository(IGenericRepository<CPK46> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK46 Save(CPK46  cpk46)
{
	cpk46 = _genericRepository.Save(cpk46);	
	return cpk46;
}
public CPK46 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}