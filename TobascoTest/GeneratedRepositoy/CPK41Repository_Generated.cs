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
	public  partial class CPK41Repository : ICPK41Repository
	{
		private IGenericRepository<CPK41> _genericRepository;
public CPK41Repository(IGenericRepository<CPK41> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK41 Save(CPK41  cpk41)
{
	cpk41 = _genericRepository.Save(cpk41);	
	return cpk41;
}
public CPK41 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}