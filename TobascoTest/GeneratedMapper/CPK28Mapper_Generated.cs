using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class CPK28Mapper : ICPK28Mapper
	{
		public CPK28 MapToObject(TobascoTest.GeneratedEntity2.CPK28 objectToMapFrom)
    {
        var objectToMapTo = new CPK28
        {
            Training = objectToMapFrom.Training,
            Duur = objectToMapFrom.Duur,
            Kosten = objectToMapFrom.Kosten,
        };

        return objectToMapTo;
    }
	
	}
}