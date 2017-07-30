using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;

namespace TobascoTest.GeneratedRepositoy
{
    public partial class FileMetOverervingRepository : IFileMetOverervingRepository
    {
private IChildObjectRepository _iChildObjectRepository;
private IChildCollectionObjectRepository _iChildCollectionObjectRepository;
private IGenericRepository<FileMetOvererving> _genericRepository;
public FileMetOverervingRepository(IChildObjectRepository iChildObjectRepository, IChildCollectionObjectRepository iChildCollectionObjectRepository, IGenericRepository<FileMetOvererving> genericRepository)
{
	_iChildObjectRepository = iChildObjectRepository;
_iChildCollectionObjectRepository = iChildCollectionObjectRepository;
_genericRepository = genericRepository;
	
}
        public FileMetOvererving Save(FileMetOvererving  filemetovererving)
{
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
