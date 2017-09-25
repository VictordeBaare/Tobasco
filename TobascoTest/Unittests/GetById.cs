using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tobasco.Model.Builders;
using Tobasco.Model;
using Tobasco.Model.Builders.DatabaseBuilders;

namespace TobascoTest.Unittests
{
    [TestClass]
    public class GetById
    {
        private Entity _entity;
        private Database _database;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new Entity
            {
                Properties = new System.Collections.Generic.List<Property> {
                    new Property
                    {
                        DataType = new DataType {Datatype = Tobasco.Enums.Datatype.Child },
                        Name = "TestId"
                    },
                    new Property
                    {
                        DataType = new DataType {Datatype = Tobasco.Enums.Datatype.Child },
                        Name = "Test2Id"
                    }
                },
                Name = "Entity"
            };
            _database = new Database();
        }


        [TestMethod]
        public void TestMethod1()
        {
            var builder = new GetByIdBuilder(_entity, _database);

            var template = builder.Build();
        }
    }
}
