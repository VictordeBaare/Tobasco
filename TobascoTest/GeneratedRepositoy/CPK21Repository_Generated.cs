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
	public  partial class CPK21Repository : ICPK21Repository
	{
		private IGenericRepository<CPK21> _genericRepository;
public CPK21Repository(IGenericRepository<CPK21> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK21 Save(CPK21  cpk21)
{
	cpk21 = _genericRepository.Save(cpk21);	
	return cpk21;
}
public CPK21 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}