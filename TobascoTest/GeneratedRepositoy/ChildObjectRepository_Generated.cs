using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    [Serializable]
    public partial class ChildObjectRepository : IChildObjectRepository
    {
        private GenericRepository<ChildObject> _genericRepository;
        public ChildObjectRepository(GenericRepository<ChildObject> genericRepository)
        {
            _genericRepository = genericRepository;
        }



        public ChildObject Save(ChildObject childobject)
        {
            childobject = _genericRepository.Save(childobject);
            return childobject;
        }

    }
}
