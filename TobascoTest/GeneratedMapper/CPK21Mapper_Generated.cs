using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class CPK21Mapper : ICPK21Mapper
	{
		public CPK21 MapToObject(TobascoTest.GeneratedEntity2.CPK21 objectToMapFrom)
    {
        var objectToMapTo = new CPK21
        {
            Training = objectToMapFrom.Training,
            Duur = objectToMapFrom.Duur,
            Kosten = objectToMapFrom.Kosten,
        };

        return objectToMapTo;
    }
	
	}
}