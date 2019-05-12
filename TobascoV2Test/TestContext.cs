using System;
using TobascoV2.Context;
using TobascoV2.Context.Base;

namespace TobascoV2Test
{
    public class TestContext : ITobascoContext
    {
        public EntityContext EntityContext => GetEntityContext();

        private EntityContext GetEntityContext()
        {
            var entityContext = new EntityContext { EntityLocation = new FileLocation() };
            entityContext.EntityLocation.Folder = "Entities";
            entityContext.EntityLocation.Project = "TobascoV2Test";

            entityContext.BaseClass = "";

            return entityContext;
        }

        public DatabaseContext DatabaseContext => throw new System.NotImplementedException();

        public bool AddBusinessRules => true;

        public string BaseClass => string.Empty;
    }
}
