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
	public  partial class CPK38Repository : ICPK38Repository
	{
		private IGenericRepository<CPK38> _genericRepository;
public CPK38Repository(IGenericRepository<CPK38> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK38 Save(CPK38  cpk38)
{
	cpk38 = _genericRepository.Save(cpk38);	
	return cpk38;
}
public CPK38 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}