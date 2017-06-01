using System;
using TobascoTest.IGenerateRepository;
using TobascoTest.GeneratedEntity;
using Tobasco;

namespace TobascoTest.IGenerateRepository
{
    public interface IChildObjectRepository
    {
        ChildObject Save(ChildObject childobject);
    }
}
