using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    public partial class ChildObjectDacRepository : IChildObjectDacRepository
    {
        private readonly GenericRepository<ChildObjectDac> _genericRepository;
        public ChildObjectDacRepository(GenericRepository<ChildObjectDac> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public ChildObjectDac Save(ChildObjectDac childobjectdac)
        {
            childobjectdac = _genericRepository.Save(childobjectdac);
            return childobjectdac;
        }


    }
}
