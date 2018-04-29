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
	public  partial class CPK16Repository : ICPK16Repository
	{
		private IGenericRepository<CPK16> _genericRepository;
public CPK16Repository(IGenericRepository<CPK16> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK16 Save(CPK16  cpk16)
{
	cpk16 = _genericRepository.Save(cpk16);	
	return cpk16;
}
public CPK16 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}