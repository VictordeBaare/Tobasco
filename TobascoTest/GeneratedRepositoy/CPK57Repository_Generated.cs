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
	public  partial class CPK57Repository : ICPK57Repository
	{
		private IGenericRepository<CPK57> _genericRepository;
public CPK57Repository(IGenericRepository<CPK57> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK57 Save(CPK57  cpk57)
{
	cpk57 = _genericRepository.Save(cpk57);	
	return cpk57;
}
public CPK57 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}