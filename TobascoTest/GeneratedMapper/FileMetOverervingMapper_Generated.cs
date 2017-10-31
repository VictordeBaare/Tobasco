using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class FileMetOverervingMapper : IFileMetOverervingMapper
	{
		private IChildCollectionObjectMapper _childCollectionObjectMapper;
public FileMetOverervingMapper(IChildCollectionObjectMapper childCollectionObjectMapper)
{
	_childCollectionObjectMapper = childCollectionObjectMapper;
	
}		
				
		public FileMetOvererving MapToObject(TobascoTest.GeneratedEntity2.FileMetOvererving objectToMapFrom)
    {
        var objectToMapTo = new FileMetOvererving
        {
            TestChildProp1 = objectToMapFrom.TestChildProp1,
            TestChildProp2 = objectToMapFrom.TestChildProp2,
            TestChildProp3 = objectToMapFrom.TestChildProp3,
            TestChildProp4 = objectToMapFrom.TestChildProp4,
            TestChildProp5 = objectToMapFrom.TestChildProp5,
            TestChildProp6 = objectToMapFrom.TestChildProp6,
        };

        foreach(var property in objectToMapFrom.TestChildProp8)
        {
            objectToMapTo.TestChildProp8.Add(_childCollectionObjectMapper.MapToObject(property));
        }

        return objectToMapTo;
    }
	
	}
}