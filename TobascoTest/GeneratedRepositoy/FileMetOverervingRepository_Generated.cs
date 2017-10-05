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

        public FileMetOvererving Save(FileMetOvererving filemetovererving)
        {
            if (filemetovererving.TestChildProp7 != null)
            {
                filemetovererving.TestChildProp7 = _iChildObjectRepository.Save(filemetovererving.TestChildProp7);
            }

            filemetovererving = _genericRepository.Save(filemetovererving);
            foreach (var toSaveItem in filemetovererving.TestChildProp8)
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
            var TestChildProp7Dict = ChildObjectRepository.Read(reader);
            var TestChildProp9Dict = ChildObjectRepository.Read(reader);

            var TestChildProp8Dict = ChildCollectionObjectRepository.Read(reader);

            var items = reader.Read((FileMetOvererving item, long TestChildProp7, long TestChildProp9) =>
            {
                if (TestChildProp7Dict.ContainsKey(TestChildProp7))
                {
                    item.TestChildProp7 = TestChildProp7Dict[TestChildProp7];
                }

                if (TestChildProp9Dict.ContainsKey(TestChildProp9))
                {
                    item.TestChildProp9 = TestChildProp9Dict[TestChildProp9];
                }


                foreach (var obj in TestChildProp8Dict.Values.Where(x => x.FileMetOverervingId == item.Id))
                {
                    item.TestChildProp8.Add(obj);
                }


                return item;
            }, splitOn: "TestChildProp7,TestChildProp9");

            return items.ToDictionary(x => x.Id);
        }
    }
}