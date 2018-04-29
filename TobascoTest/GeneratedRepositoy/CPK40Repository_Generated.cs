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
	public  partial class CPK40Repository : ICPK40Repository
	{
		private IGenericRepository<CPK40> _genericRepository;
public CPK40Repository(IGenericRepository<CPK40> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK40 Save(CPK40  cpk40)
{
	cpk40 = _genericRepository.Save(cpk40);	
	return cpk40;
}
public CPK40 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}