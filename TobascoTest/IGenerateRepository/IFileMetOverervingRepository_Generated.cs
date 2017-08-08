using System;
using TobascoTest.IGenerateRepository;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
    public partial interface IFileMetOverervingRepository
    {


        FileMetOvererving Save(FileMetOvererving filemetovererving);
        FileMetOvererving GetById(long id);
    }
}