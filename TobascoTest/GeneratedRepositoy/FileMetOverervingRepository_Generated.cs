using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    [Serializable]
    public partial class FileMetOverervingRepository : IFileMetOverervingRepository
    {
        private GenericRepository<FileMetOvererving> _genericRepository;
        private IChildObjectRepository _iChildObjectRepository;
        private IChildCollectionObjectRepository _iChildCollectionObjectRepository;
        public FileMetOverervingRepository(GenericRepository<FileMetOvererving> genericRepository, IChildObjectRepository iChildObjectRepository, IChildCollectionObjectRepository iChildCollectionObjectRepository)
        {
            _genericRepository = genericRepository;
            _iChildObjectRepository = iChildObjectRepository;
            _iChildCollectionObjectRepository = iChildCollectionObjectRepository;
        }



        public FileMetOvererving Save(FileMetOvererving filemetovererving)
        {
            filemetovererving.TestChildProp7 = _iChildObjectRepository.Save(filemetovererving.TestChildProp7);
            filemetovererving = _genericRepository.Save(filemetovererving);
            foreach(var toSaveItem in filemetovererving.TestChildProp8)
            {
                toSaveItem.FileMetOverervingId = filemetovererving.Id;
                _iChildCollectionObjectRepository.Save(toSaveItem);
            }
            return filemetovererving;
        }

    }
}
