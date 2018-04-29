using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class CPK34Mapper : ICPK34Mapper
	{
		public CPK34 MapToObject(TobascoTest.GeneratedEntity2.CPK34 objectToMapFrom)
    {
        var objectToMapTo = new CPK34
        {
            Training = objectToMapFrom.Training,
            Duur = objectToMapFrom.Duur,
            Kosten = objectToMapFrom.Kosten,
        };

        return objectToMapTo;
    }
	
	}
}