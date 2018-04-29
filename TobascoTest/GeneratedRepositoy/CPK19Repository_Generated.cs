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
	public  partial class CPK19Repository : ICPK19Repository
	{
		private IGenericRepository<CPK19> _genericRepository;
public CPK19Repository(IGenericRepository<CPK19> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK19 Save(CPK19  cpk19)
{
	cpk19 = _genericRepository.Save(cpk19);	
	return cpk19;
}
public CPK19 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}