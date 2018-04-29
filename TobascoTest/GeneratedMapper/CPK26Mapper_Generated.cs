using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class CPK26Mapper : ICPK26Mapper
	{
		public CPK26 MapToObject(TobascoTest.GeneratedEntity2.CPK26 objectToMapFrom)
    {
        var objectToMapTo = new CPK26
        {
            Training = objectToMapFrom.Training,
            Duur = objectToMapFrom.Duur,
            Kosten = objectToMapFrom.Kosten,
        };

        return objectToMapTo;
    }
	
	}
}