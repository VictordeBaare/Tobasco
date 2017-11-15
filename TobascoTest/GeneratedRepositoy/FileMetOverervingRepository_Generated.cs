using System;
using System.CodeDom.Compiler;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using static Dapper.SqlMapper;

namespace TobascoTest.GeneratedRepositoy
{
	[GeneratedCode("Tobasco", "1.0.0.0")]
	public  partial class FileMetOverervingRepository : IFileMetOverervingRepository
	{
		private IGenericRepository<FileMetOvererving> _genericRepository;
private IChildObjectRepository _iChildObjectRepository;
private IChildCollectionObjectRepository _iChildCollectionObjectRepository;
public FileMetOverervingRepository(IGenericRepository<FileMetOvererving> genericRepository, IChildObjectRepository iChildObjectRepository, IChildCollectionObjectRepository iChildCollectionObjectRepository)
{
	_genericRepository = genericRepository;
_iChildObjectRepository = iChildObjectRepository;
_iChildCollectionObjectRepository = iChildCollectionObjectRepository;
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