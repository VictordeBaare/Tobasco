using System;
using TobascoTest.GeneratedMapper;
using TobascoTest.GeneratedEntity;

namespace TobascoTest.GeneratedMapper
{
    [Serializable]
    public class ChildObjectMapper : IChildObjectMapper
    {
        public ChildObjectMapper()
        {
        }



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
