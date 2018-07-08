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
	public  partial class CPK25Repository : ICPK25Repository
	{
		private IGenericRepository<CPK25> _genericRepository;
public CPK25Repository(IGenericRepository<CPK25> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK25 Save(CPK25  cpk25)
{
	cpk25 = _genericRepository.Save(cpk25);	
	return cpk25;
}
public CPK25 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}