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
	public  partial class CPK52Repository : ICPK52Repository
	{
		private IGenericRepository<CPK52> _genericRepository;
public CPK52Repository(IGenericRepository<CPK52> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK52 Save(CPK52  cpk52)
{
	cpk52 = _genericRepository.Save(cpk52);	
	return cpk52;
}
public CPK52 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}