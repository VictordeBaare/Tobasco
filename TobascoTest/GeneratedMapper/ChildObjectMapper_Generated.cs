using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class ChildObjectMapper : IChildObjectMapper
	{
		public ChildObject MapToObject(TobascoTest.GeneratedEntity2.ChildObject objectToMapFrom)
    {
        var objectToMapTo = new ChildObject
        {
            TestChildProp1 = objectToMapFrom.TestChildProp1,
        };

        return objectToMapTo;
    }
	
	}
}