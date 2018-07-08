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
	public  partial class CPK29Repository : ICPK29Repository
	{
		private IGenericRepository<CPK29> _genericRepository;
public CPK29Repository(IGenericRepository<CPK29> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK29 Save(CPK29  cpk29)
{
	cpk29 = _genericRepository.Save(cpk29);	
	return cpk29;
}
public CPK29 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}