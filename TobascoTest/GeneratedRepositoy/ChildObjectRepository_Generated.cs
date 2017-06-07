using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    [Serializable]
    public partial class ChildObjectRepository : IChildObjectRepository
    {
        private GetDacFunc _xDacFunc;
        private delegate ChildObjectDac GetDacFunc(ChildObject childobject);
        private readonly GenericRepository<ChildObject> _genericRepository;
        private readonly IChildObjectDacRepository _iChildObjectDacRepository;
        public ChildObjectRepository(GenericRepository<ChildObject> genericRepository, IChildObjectDacRepository iChildObjectDacRepository)
        {
            _genericRepository = genericRepository;
            _iChildObjectDacRepository = iChildObjectDacRepository;
            OnCreated();
        }

        public ChildObject Save(ChildObject childobject)
        {
            if (_xDacFunc != null)
            {
                _iChildObjectDacRepository.Save(_xDacFunc(childobject));
            }
            childobject = _genericRepository.Save(childobject);
            return childobject;
        }


        partial void OnCreated();

    }
}
