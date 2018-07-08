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
	public partial interface ICPK17Repository
	{
			
		CPK17 Save(CPK17 cpk17);
CPK17 GetById(long id);	
	}
}