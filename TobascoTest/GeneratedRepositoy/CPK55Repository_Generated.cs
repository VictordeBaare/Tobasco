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
	public  partial class CPK55Repository : ICPK55Repository
	{
		private IGenericRepository<CPK55> _genericRepository;
public CPK55Repository(IGenericRepository<CPK55> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK55 Save(CPK55  cpk55)
{
	cpk55 = _genericRepository.Save(cpk55);	
	return cpk55;
}
public CPK55 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}