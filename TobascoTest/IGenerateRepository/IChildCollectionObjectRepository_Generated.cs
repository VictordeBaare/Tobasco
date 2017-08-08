using System;
using TobascoTest.IGenerateRepository;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
    public partial interface IChildCollectionObjectRepository
    {


        ChildCollectionObject Save(ChildCollectionObject childcollectionobject);
        ChildCollectionObject GetById(long id);
    }
}