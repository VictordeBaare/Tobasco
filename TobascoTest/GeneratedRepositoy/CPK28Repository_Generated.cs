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
	public  partial class CPK28Repository : ICPK28Repository
	{
		private IGenericRepository<CPK28> _genericRepository;
public CPK28Repository(IGenericRepository<CPK28> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK28 Save(CPK28  cpk28)
{
	cpk28 = _genericRepository.Save(cpk28);	
	return cpk28;
}
public CPK28 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}