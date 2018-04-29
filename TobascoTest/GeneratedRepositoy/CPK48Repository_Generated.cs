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
	public  partial class CPK48Repository : ICPK48Repository
	{
		private IGenericRepository<CPK48> _genericRepository;
public CPK48Repository(IGenericRepository<CPK48> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK48 Save(CPK48  cpk48)
{
	cpk48 = _genericRepository.Save(cpk48);	
	return cpk48;
}
public CPK48 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}