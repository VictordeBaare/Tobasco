using System;
using TobascoTest.IGenerateRepository;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
    public partial interface IChildObjectRepository
    {


        ChildObject Save(ChildObject childobject);
        ChildObject GetById(long id);
    }
}