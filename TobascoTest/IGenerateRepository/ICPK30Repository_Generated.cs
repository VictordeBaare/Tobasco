using System;
using System.CodeDom.Compiler;
using TobascoTest.IGenerateRepository;
using static Dapper.SqlMapper;
using Dapper;
using System.Linq;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public partial interface ICPK30Repository
	{
			
		CPK30 Save(CPK30 cpk30);
CPK30 GetById(long id);	
	}
}