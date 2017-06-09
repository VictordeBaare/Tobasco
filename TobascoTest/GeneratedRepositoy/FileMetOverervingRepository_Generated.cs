using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;

namespace TobascoTest.GeneratedRepositoy
{
    public partial class FileMetOverervingRepository : IFileMetOverervingRepository
    {
        private GetDacFunc _xDacFunc;
        private delegate IEnumerable<FileMetOverervingDac> GetDacFunc(FileMetOvererving filemetovererving);
        private readonly IGenericRepository<FileMetOvererving> _genericRepository;
        private readonly IFileMetOverervingDacRepository _iFileMetOverervingDacRepository;
        private readonly IChildObjectRepository _iChildObjectRepository;
        private readonly IChildCollectionObjectRepository _iChildCollectionObjectRepository;
        public FileMetOverervingRepository(IGenericRepository<FileMetOvererving> genericRepository, IFileMetOverervingDacRepository iFileMetOverervingDacRepository, IChildObjectRepository iChildObjectRepository, IChildCollectionObjectRepository iChildCollectionObjectRepository)
        {
            _genericRepository = genericRepository;
            _iFileMetOverervingDacRepository = iFileMetOverervingDacRepository;
            _iChildObjectRepository = iChildObjectRepository;
            _iChildCollectionObjectRepository = iChildCollectionObjectRepository;
            OnCreated();
        }

        partial void OnCreated();

        public FileMetOvererving Save(FileMetOvererving filemetovererving)
        {
            if (_xDacFunc != null)
            {
                foreach(var securityItem in _xDacFunc(filemetovererving))
                {
                    _iFileMetOverervingDacRepository.Save(securityItem);
                }
            }
            if (filemetovererving.TestChildProp7 != null)
            {
                filemetovererving.TestChildProp7 = _iChildObjectRepository.Save(filemetovererving.TestChildProp7);
            }
            filemetovererving = _genericRepository.Save(filemetovererving);
            foreach(var toSaveItem in filemetovererving.TestChildProp8)
            {
                toSaveItem.FileMetOverervingId = filemetovererving.Id;
                _iChildCollectionObjectRepository.Save(toSaveItem);
            }
            return filemetovererving;
        }


        public FileMetOvererving GetById(long id)
        {
            return _genericRepository.GetById(id);
        }


    }
}
