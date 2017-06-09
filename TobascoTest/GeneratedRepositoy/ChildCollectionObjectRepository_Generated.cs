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
        private readonly IGenericRepository<ChildCollectionObject> _genericRepository;
        private readonly IChildCollectionObjectDacRepository _iChildCollectionObjectDacRepository;
        public ChildCollectionObjectRepository(IGenericRepository<ChildCollectionObject> genericRepository, IChildCollectionObjectDacRepository iChildCollectionObjectDacRepository)
        {
            _genericRepository = genericRepository;
            _iChildCollectionObjectDacRepository = iChildCollectionObjectDacRepository;
            OnCreated();
        }

        partial void OnCreated();

        public ChildCollectionObject Save(ChildCollectionObject childcollectionobject)
        {
            if (_xDacFunc != null)
            {
                _iChildCollectionObjectDacRepository.Save(_xDacFunc(childcollectionobject));
            }
            childcollectionobject = _genericRepository.Save(childcollectionobject);
            return childcollectionobject;
        }


        public ChildCollectionObject GetById(long id)
        {
            return _genericRepository.GetById(id);
        }

        partial void OnCreated()
        {
            _xDacFunc = XDacFunc;
        }

        private ChildCollectionObjectDac XDacFunc(ChildCollectionObject childcollectionobject)
        {
            throw new NotImplementedException();
        }
    }
}
