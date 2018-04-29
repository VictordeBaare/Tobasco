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
	public  partial class CPK34Repository : ICPK34Repository
	{
		private IGenericRepository<CPK34> _genericRepository;
public CPK34Repository(IGenericRepository<CPK34> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK34 Save(CPK34  cpk34)
{
	cpk34 = _genericRepository.Save(cpk34);	
	return cpk34;
}
public CPK34 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}