using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;

namespace TobascoTest.GeneratedRepositoy
{
    public partial class ChildCollectionObjectRepository : IChildCollectionObjectRepository
    {
        private GetDacFunc _xDacFunc;
        private delegate IEnumerable<ChildCollectionObjectDac> GetDacFunc(ChildCollectionObject childcollectionobject);
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
                foreach(var securityItem in _xDacFunc(childcollectionobject))
                {
                    _iChildCollectionObjectDacRepository.Save(securityItem);
                }
            }
            childcollectionobject = _genericRepository.Save(childcollectionobject);
            return childcollectionobject;
        }


        public ChildCollectionObject GetById(long id)
        {
            return _genericRepository.GetById(id);
        }


    }
}
