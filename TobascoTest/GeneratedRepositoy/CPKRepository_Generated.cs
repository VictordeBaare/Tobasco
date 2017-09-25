using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;

namespace TobascoTest.GeneratedRepositoy
{
    [GeneratedCode("Tobasco", "1.0.0.0")]
    public partial class CPKRepository : ICPKRepository
    {
        private IGenericRepository<CPK> _genericRepository;
        public CPKRepository(IGenericRepository<CPK> genericRepository)
        {
            _genericRepository = genericRepository;

        }

        public CPK Save(CPK cpk)
        {

            cpk = _genericRepository.Save(cpk);

            return cpk;
        }
        public CPK GetById(long id)
        {
            return _genericRepository.GetById(id);
        }

    }
}