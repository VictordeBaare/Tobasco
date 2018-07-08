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
	public  partial class CPK11Repository : ICPK11Repository
	{
		private IGenericRepository<CPK11> _genericRepository;
public CPK11Repository(IGenericRepository<CPK11> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK11 Save(CPK11  cpk11)
{
	cpk11 = _genericRepository.Save(cpk11);	
	return cpk11;
}
public CPK11 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}