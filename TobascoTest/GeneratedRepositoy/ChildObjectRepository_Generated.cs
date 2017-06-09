using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;

namespace TobascoTest.GeneratedRepositoy
{
    public partial class ChildObjectRepository : IChildObjectRepository
    {
        private GetDacFunc _xDacFunc;
        private delegate IEnumerable<ChildObjectDac> GetDacFunc(ChildObject childobject);
        private readonly IGenericRepository<ChildObject> _genericRepository;
        private readonly IChildObjectDacRepository _iChildObjectDacRepository;
        public ChildObjectRepository(IGenericRepository<ChildObject> genericRepository, IChildObjectDacRepository iChildObjectDacRepository)
        {
            _genericRepository = genericRepository;
            _iChildObjectDacRepository = iChildObjectDacRepository;
            OnCreated();
        }

        partial void OnCreated();

        public ChildObject Save(ChildObject childobject)
        {
            if (_xDacFunc != null)
            {
                foreach(var securityItem in _xDacFunc(childobject))
                {
                    _iChildObjectDacRepository.Save(securityItem);
                }
            }
            childobject = _genericRepository.Save(childobject);
            return childobject;
        }


        public ChildObject GetById(long id)
        {
            return _genericRepository.GetById(id);
        }


    }
}
