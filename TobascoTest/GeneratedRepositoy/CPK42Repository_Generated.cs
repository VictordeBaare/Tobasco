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
	public  partial class CPK42Repository : ICPK42Repository
	{
		private IGenericRepository<CPK42> _genericRepository;
public CPK42Repository(IGenericRepository<CPK42> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK42 Save(CPK42  cpk42)
{
	cpk42 = _genericRepository.Save(cpk42);	
	return cpk42;
}
public CPK42 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}