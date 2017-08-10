using System;
using System.CodeDom.Compiler;
using TobascoTest.IGenerateRepository;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
    [GeneratedCode("Tobasco", "1.0.0.0")]
    public partial interface IChildCollectionObjectRepository
    {


        ChildCollectionObject Save(ChildCollectionObject childcollectionobject);
        ChildCollectionObject GetById(long id);
    }
}