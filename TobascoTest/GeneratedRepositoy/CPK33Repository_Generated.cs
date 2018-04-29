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
	public  partial class CPK33Repository : ICPK33Repository
	{
		private IGenericRepository<CPK33> _genericRepository;
public CPK33Repository(IGenericRepository<CPK33> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK33 Save(CPK33  cpk33)
{
	cpk33 = _genericRepository.Save(cpk33);	
	return cpk33;
}
public CPK33 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}