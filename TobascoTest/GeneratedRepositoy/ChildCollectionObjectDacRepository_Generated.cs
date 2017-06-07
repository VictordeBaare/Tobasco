using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    [Serializable]
    public partial class ChildCollectionObjectDacRepository : IChildCollectionObjectDacRepository
    {
        private readonly GenericRepository<ChildCollectionObjectDac> _genericRepository;
        public ChildCollectionObjectDacRepository(GenericRepository<ChildCollectionObjectDac> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public ChildCollectionObjectDac Save(ChildCollectionObjectDac childcollectionobjectdac)
        {
            childcollectionobjectdac = _genericRepository.Save(childcollectionobjectdac);
            return childcollectionobjectdac;
        }


    }
}
