using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class ChildCollectionObjectMapper : IChildCollectionObjectMapper
	{
		public ChildCollectionObject MapToObject(TobascoTest.GeneratedEntity2.ChildCollectionObject objectToMapFrom)
    {
        var objectToMapTo = new ChildCollectionObject
        {
            TestChildProp1 = objectToMapFrom.TestChildProp1,
            TestChildProp2 = objectToMapFrom.TestChildProp2,
            FileMetOverervingId = objectToMapFrom.FileMetOverervingId,
        };

        return objectToMapTo;
    }
	
	}
}