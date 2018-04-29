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
	public  partial class CPK39Repository : ICPK39Repository
	{
		private IGenericRepository<CPK39> _genericRepository;
public CPK39Repository(IGenericRepository<CPK39> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK39 Save(CPK39  cpk39)
{
	cpk39 = _genericRepository.Save(cpk39);	
	return cpk39;
}
public CPK39 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}