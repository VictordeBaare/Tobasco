using System;
using Tobasco;
using TobascoTest.GeneratedEntity;
using TobascoTest.IGenerateRepository;

namespace TobascoTest.GeneratedRepositoy
{
    public partial class FileMetOverervingDacRepository : IFileMetOverervingDacRepository
    {
        private readonly GenericRepository<FileMetOverervingDac> _genericRepository;
        public FileMetOverervingDacRepository(GenericRepository<FileMetOverervingDac> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public FileMetOverervingDac Save(FileMetOverervingDac filemetoverervingdac)
        {
            filemetoverervingdac = _genericRepository.Save(filemetoverervingdac);
            return filemetoverervingdac;
        }


    }
}
