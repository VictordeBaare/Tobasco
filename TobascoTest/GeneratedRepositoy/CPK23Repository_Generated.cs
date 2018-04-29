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
	public  partial class CPK23Repository : ICPK23Repository
	{
		private IGenericRepository<CPK23> _genericRepository;
public CPK23Repository(IGenericRepository<CPK23> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK23 Save(CPK23  cpk23)
{
	cpk23 = _genericRepository.Save(cpk23);	
	return cpk23;
}
public CPK23 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}