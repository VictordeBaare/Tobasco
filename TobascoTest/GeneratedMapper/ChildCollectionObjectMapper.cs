using System;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
    [Serializable]
    public class ChildCollectionObjectMapper : IChildCollectionObjectMapper
    {
        public ChildCollectionObjectMapper()
        {
        }



        public ChildCollectionObject MapToObject(TobascoTest.GeneratedEntity2.ChildCollectionObject objectToMapFrom)
    {
           var objectToMapTo = new ChildCollectionObject 
           {
               TestChildProp1 = objectToMapFrom.TestChildProp1,
               FileMetOverervingId = objectToMapFrom.FileMetOverervingId,
           };

            return objectToMapTo;
        }

    }
}
