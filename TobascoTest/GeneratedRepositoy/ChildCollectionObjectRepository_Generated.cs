using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    [Serializable]
    public partial class ChildCollectionObjectRepository : IChildCollectionObjectRepository
    {
        private GetDacFunc _xDacFunc;
        private delegate ChildCollectionObjectDac GetDacFunc(ChildCollectionObject childcollectionobject);
        private readonly GenericRepository<ChildCollectionObject> _genericRepository;
        private readonly IChildCollectionObjectDacRepository _iChildCollectionObjectDacRepository;
        public ChildCollectionObjectRepository(GenericRepository<ChildCollectionObject> genericRepository, IChildCollectionObjectDacRepository iChildCollectionObjectDacRepository)
        {
            _genericRepository = genericRepository;
            _iChildCollectionObjectDacRepository = iChildCollectionObjectDacRepository;
            OnCreated();
        }

        public ChildCollectionObject Save(ChildCollectionObject childcollectionobject)
        {
            if (_xDacFunc != null)
            {
                _iChildCollectionObjectDacRepository.Save(_xDacFunc(childcollectionobject));
            }
            childcollectionobject = _genericRepository.Save(childcollectionobject);
            return childcollectionobject;
        }


        partial void OnCreated();

    }
}
