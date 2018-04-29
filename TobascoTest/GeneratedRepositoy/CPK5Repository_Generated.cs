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
	public  partial class CPK5Repository : ICPK5Repository
	{
		private IGenericRepository<CPK5> _genericRepository;
public CPK5Repository(IGenericRepository<CPK5> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK5 Save(CPK5  cpk5)
{
	cpk5 = _genericRepository.Save(cpk5);	
	return cpk5;
}
public CPK5 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}