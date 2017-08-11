using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;

namespace TobascoTest.GeneratedRepositoy
{
    [GeneratedCode("Tobasco", "1.0.0.0")]
    public partial class ChildObjectRepository : IChildObjectRepository
    {
        private IGenericRepository<ChildObject> _genericRepository;
        public ChildObjectRepository(IGenericRepository<ChildObject> genericRepository)
        {
            _genericRepository = genericRepository;

        }

        public ChildObject Save(ChildObject childobject)
        {

            childobject = _genericRepository.Save(childobject);

            return childobject;
        }
        public ChildObject GetById(long id)
        {
            return _genericRepository.GetById(id);
        }

    }
}