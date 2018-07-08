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
	public  partial class CPK35Repository : ICPK35Repository
	{
		private IGenericRepository<CPK35> _genericRepository;
public CPK35Repository(IGenericRepository<CPK35> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK35 Save(CPK35  cpk35)
{
	cpk35 = _genericRepository.Save(cpk35);	
	return cpk35;
}
public CPK35 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}