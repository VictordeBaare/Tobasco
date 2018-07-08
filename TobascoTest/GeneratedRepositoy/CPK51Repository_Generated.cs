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
	public  partial class CPK51Repository : ICPK51Repository
	{
		private IGenericRepository<CPK51> _genericRepository;
public CPK51Repository(IGenericRepository<CPK51> genericRepository)
{
	_genericRepository = genericRepository;
}		
		public CPK51 Save(CPK51  cpk51)
{
	cpk51 = _genericRepository.Save(cpk51);	
	return cpk51;
}
public CPK51 GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}