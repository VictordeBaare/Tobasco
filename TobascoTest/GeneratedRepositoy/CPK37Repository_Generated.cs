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
	public  partial class CPK37Repository : ICPK37Repository
	{
		private IGenericRepository<CPK37> _genericRepository;
public CPK37Repository(IGenericRepository<CPK37> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK37 Save(CPK37  cpk37)
{
	cpk37 = _genericRepository.Save(cpk37);	
	return cpk37;
}
public CPK37 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}