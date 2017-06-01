using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    [Serializable]
    public partial class ChildCollectionObjectRepository : IChildCollectionObjectRepository
    {
        private GenericRepository<ChildCollectionObject> _genericRepository;
        public ChildCollectionObjectRepository(GenericRepository<ChildCollectionObject> genericRepository)
        {
            _genericRepository = genericRepository;
        }



        public ChildCollectionObject Save(ChildCollectionObject childcollectionobject)
        {
            childcollectionobject = _genericRepository.Save(childcollectionobject);
            return childcollectionobject;
        }

    }
}
