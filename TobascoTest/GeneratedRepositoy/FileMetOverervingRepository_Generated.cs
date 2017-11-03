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
		private IChildCollectionObjectRepository _iChildCollectionObjectRepository;
private IGenericRepository<FileMetOvererving> _genericRepository;
public FileMetOverervingRepository(IChildCollectionObjectRepository iChildCollectionObjectRepository, IGenericRepository<FileMetOvererving> genericRepository)
{
	_iChildCollectionObjectRepository = iChildCollectionObjectRepository;
_genericRepository = genericRepository;
}		
				
		public FileMetOvererving Save(FileMetOvererving  filemetovererving)
{
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

public FileMetOvererving GetFullObjectById(long id)
{
    var parameters = new DynamicParameters();
    parameters.Add("id", id);
    return _genericRepository.QueryMultiple("[dbo].[FileMetOvererving_GetFullById]", parameters, x => Read(x).Values).SingleOrDefault();
}
internal static Dictionary<long, FileMetOvererving> Read(GridReader reader)
{
	var TestChildProp8Dict = ChildCollectionObjectRepository.Read(reader);

    var items = reader.Read((FileMetOvererving item, FileMetOvererving returnItem) =>
    {
        foreach (var obj in TestChildProp8Dict.Values.Where(x => x.FileMetOverervingId == returnItem.Id))
{
returnItem.TestChildProp8.Add(obj);
}

        return returnItem;
    });

    return items.ToDictionary(x => x.Id);        
}	
	}
}