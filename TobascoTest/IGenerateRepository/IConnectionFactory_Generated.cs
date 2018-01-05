using System;
using System.CodeDom.Compiler;
using System.Data.SqlClient;

namespace TobascoTest.IGenerateRepository
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public partial interface IConnectionFactory
	{
		
			
		SqlConnection GetConnection();	
	}
}