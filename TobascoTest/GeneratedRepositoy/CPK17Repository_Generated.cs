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
	public  partial class CPK17Repository : ICPK17Repository
	{
		private IGenericRepository<CPK17> _genericRepository;
public CPK17Repository(IGenericRepository<CPK17> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK17 Save(CPK17  cpk17)
{
	cpk17 = _genericRepository.Save(cpk17);	
	return cpk17;
}
public CPK17 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}